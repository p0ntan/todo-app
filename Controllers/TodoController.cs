using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Data;
using Microsoft.EntityFrameworkCore;
namespace TodoApi.Controllers;

[ApiController]
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

        if(todo == null)
            return NotFound();

        return todo;
    }

    [HttpGet]
    public ActionResult<List<Todo>> GetAll([FromQuery(Name = "date")] string? date)
    {
        List<Todo> todos;
        
        if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out DateTime dateTime))
        {
            todos = [.. _context.Todos.Where(t => t.Date.Date == dateTime.Date)];
        }
        else
        {
            todos = [.. _context.Todos];
        }

        return todos;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Create(TodoCreateDto todoDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Todo todo = new()
        {
            Title = todoDto.Title,
            Description = todoDto.Description,
            Finished = todoDto.Finished,
            Date = todoDto.Date?.Date ?? DateTime.Now,
            UserId = 1  // TODO add userid from JWT
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
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
