using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models;

public class RefreshToken
{
  [Key]
  public int Id { get; set; }
  public string? Token { get; set; }
  public DateTime ExpiryDate { get; set; }
  public int UserId { get; set; }
}
