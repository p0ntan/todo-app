using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Data;
using Microsoft.EntityFrameworkCore;
namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]s")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly TodoContext _context;

    public UserController(
        ILogger<UserController> logger,
        TodoContext todoContext
    )
    {
        _logger = logger;
        _context = todoContext;
    }

    [HttpGet("{id}")]
    public ActionResult<Todo> Get(int id)
    {
        Todo? todo = _context.Todos.Find(id);

        if(todo == null)
            return NotFound();

        return todo;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Todo todo)  // TODO use DTO so that user-id can't be changed, or set it with id from jwt.
    {
        if (id != todo.Id)
        {
            return BadRequest(new { error = "Id does not match" });
        }
        else if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        else if (!_context.Todos.Any(t => t.Id == id))
        {
            return NotFound();
        }

        _context.Attach(todo).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Todo? todo = _context.Todos.Find(id);

        if (todo == null)
        {
            return NotFound();
        }

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return Ok(new { message  = $"Todo with id {id} deleted."});
    }
}
