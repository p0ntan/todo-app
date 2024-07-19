using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class User
{
  [Key]
  public required int UserId { get; set; }
  public required string FirstName { get; set; }
  public required string LastName { get; set; }

  [Required(ErrorMessage = "Email is required")]
  public required string Email { get; set; }
  public ICollection<Todo>? Todos { get; set; }
  public string? RefreshToken { get; set; }
}
