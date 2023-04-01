using System.ComponentModel.DataAnnotations;

namespace Backend.Features.Users.Views;

public class UserRequest
{
    [Required] [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    public string RoleId { get; set; }
}