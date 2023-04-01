using System.ComponentModel.DataAnnotations;

namespace Backend.Auth.Users;

public class UserView
{
    public string Id { get; set; } = string.Empty;
    
    [EmailAddress] public string Email { get; set; } = string.Empty;

    public string RoleId { get; set; } = string.Empty;
}