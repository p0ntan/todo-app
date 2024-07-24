using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Data;
namespace TodoApi.Controllers;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize(Policy = "AccessToken")]
[Route("[controller]s")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly TodoContext _context;

    private IConfiguration _config;

    public UserController(
        ILogger<UserController> logger,
        TodoContext todoContext,
        IConfiguration config
    )
    {
        _logger = logger;
        _context = todoContext;
        _config = config;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UserBase userData)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        if (id != userId)
        {
            return BadRequest(new { error = new {detail = "Id does not match" }});
        }
        else if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = _context.Users.Find(id);

        if (user == null)
        {
            return NotFound(new { error = new {detail = "No user found in database." }});
        }

        user.FirstName = userData.FirstName;

        await _context.SaveChangesAsync();

        var newAccessToken = AuthModel.GenerateAccessToken(_config, user);

        return Ok(new { data = new { accessToken = newAccessToken, user = userData }, message = $"User with id {id} updated.", });
    }

    // [HttpDelete("{id}")]
    // public async Task<IActionResult> Delete(int id)
    // {
    //     Todo? todo = _context.Todos.Find(id);

    //     if (todo == null)
    //     {
    //         return NotFound();
    //     }

    //     _context.Todos.Remove(todo);
    //     await _context.SaveChangesAsync();

    //     return Ok(new { message  = $"Todo with id {id} deleted."});
    // }
}
