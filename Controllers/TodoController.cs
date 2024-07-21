using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Data;
using Microsoft.EntityFrameworkCore;
namespace TodoApi.Controllers;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Authorize(Policy = "AccessToken")]
[Route("[controller]s")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly TodoContext _context;

    public TodoController(
        ILogger<TodoController> logger,
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
        var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        if (todo == null)
        {
            return NotFound(new {error = new {detail = "No todo found." }});
        }
        
        else if (todo.UserId != userId)
        {
            return Unauthorized(new {error = new {detail = "Not authorized to access this todo."}});
        }

        return todo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Todo>> GetAll([FromQuery(Name = "date")] string? dateFilter)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
        var todos = _context.Todos.Where(todo => todo.UserId == userId);

        if (dateFilter != null)
        {
            if (DateTime.TryParse(dateFilter, out var date))
            {
                todos = todos.Where(todo => todo.Date.Date == date.Date);
            }
            else
            {
                return BadRequest(new {error = new {detail = "Invalid date filter. Please provide a valid date."}});
            }
        }

        return todos.ToList();
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Create(TodoCreateDto todoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);

        Todo todo = new()
        {
            Title = todoDto.Title,
            Description = todoDto.Description,
            Finished = todoDto.Finished,
            Date = todoDto.Date?.Date ?? DateTime.Now,
            UserId = userId
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Todo todo)  // TODO use DTO so that user-id can't be changed, or set it with id from jwt.
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
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
        else if (todo.UserId != userId)  // TODO almost works, need a DTO for limiting user-id access
        {
            return Unauthorized(new {error = new {detail = "Not authorized to access this todo."}});
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
