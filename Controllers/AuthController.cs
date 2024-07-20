using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly TodoContext _context;
    private IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthController(
        ILogger<AuthController> logger,
        TodoContext todoContext,
        IConfiguration config,
        IHttpClientFactory httpClientFactory
    )
    {
        _logger = logger;
        _context = todoContext;
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost()]
    public async Task<ActionResult<string>> RequestGoogle()
    {
        Request.Headers.TryGetValue("Authorization", out var bearerToken);

        if (string.IsNullOrEmpty(bearerToken))
        {
            return BadRequest("Missing access token.");
        }

        string googleAccessToken = bearerToken.ToString().Substring("Bearer ".Length).Trim();

        GoogleUserInfo googleInfo = await AuthModel.ValidateGoogleAccessToken(_httpClientFactory, googleAccessToken);
        User user = await AuthModel.LoginOrCreateUser(_context, googleInfo);
        var accessToken = AuthModel.GenerateAccessToken(_config, user);
        var refreshToken = AuthModel.GenerateRefreshToken(_config, user, out var expires);

        // TODO set up refresh token in db for logout and such
        // how will it work for users when logged in on mobile and desktop? What can separate them?

        // _context.RefreshTokens.Add(new RefreshToken
        // {
        //     UserId = user.Id,
        //     Token = refreshToken,
        //     ExpiryDate = expires
        // });
        // await _context.SaveChangesAsync();

        return Ok(new {accessToken, refreshToken });
    }

    [Authorize(Policy = "RefreshToken")]
    [HttpGet("refresh-token")]
    public ActionResult<string> RefreshToken()
    {
        int userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
        var user = _context.Users.Find(userId);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        var tokens = AuthModel.RefreshAccessToken(_config, user);
        return Ok(tokens);
    }
}
