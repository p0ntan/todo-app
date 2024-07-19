using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public interface ITodoBase
{
  public string Title { get; set; }
  public string? Description { get; set; }
  public bool Finished { get; set; }
}

public class Todo : ITodoBase
{
  [Key]
  public int TodoId { get; set; }
  public required string Title { get; set; }
  public string? Description { get; set; }
  public required bool Finished { get; set; }
  public DateTime Date { get; set; }
}

public class TodoCreateDto : ITodoBase
{
  [Required(ErrorMessage = "Title is required")]
  public required string Title { get; set; }
  public string? Description { get; set; }
  public bool Finished { get; set; } = false;
  [Required(ErrorMessage = "A date is required")]
  public DateTime Date { get; set; }
}
