namespace Backend.Features.Users.Views;

public class UserRequest
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string RoleId { get; set; }
}