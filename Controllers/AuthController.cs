using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TodoApi.Data;
using TodoApi.Models;
using System.Text;
using System.Security.Claims;

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

    // [HttpPost()]
    // public ActionResult Get()
    // {
    //     var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:AccessTokenSecret"]!));
    //     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    //     IEnumerable<Claim> claims =
    //     [
    //         new ("Name", "TodoApi"),
    //         new ("Email", "test@test.test"),
    //         new ("admin", "true", ClaimValueTypes.Boolean),
    //     ];

    //     var Sectoken = new JwtSecurityToken(_config["JWT:Issuer"],
    //         _config["JWT:Issuer"],
    //         claims,
    //         expires: DateTime.Now.AddSeconds(20),
    //         signingCredentials: credentials);

    //     var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

    //     return Ok(token);
    // }

    [HttpPost()]
    public async Task<ActionResult<string>> RequestGoogle()
    {
        Request.Headers.TryGetValue("Authorization", out var bearerToken);

        if (string.IsNullOrEmpty(bearerToken))
        {
            return BadRequest("Missing access token.");
        }

        string googleAccessToken = bearerToken.ToString().Substring("Bearer ".Length).Trim();

        GoogleUserInfo googleInfo = await AuthModel.RequestGoogle(_httpClientFactory, googleAccessToken);
        User user;

        if (!_context.Users.Any(u => u.Email == googleInfo.Email))
        {
            user = new User()
            {
                Email = googleInfo.Email,
                FirstName = googleInfo.GivenName,
                LastName = googleInfo.FamilyName
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        else
        {
            user = _context.Users.Where(u => u.Email == googleInfo.Email).First();
        }

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
}
