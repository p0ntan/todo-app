using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;

public class UserBase
{
  [Required(ErrorMessage = "A first name is required")]
  public string FirstName { get; set; }
}

public class User: UserBase
{
  [Key]
  public int Id { get; set; }

  [Required(ErrorMessage = "Email is required")]
  public string LastName { get; set; }
  public required string Email { get; set; }
  public ICollection<Todo>? Todos { get; set; }
  public RefreshToken? RefreshToken { get; set; }
}
