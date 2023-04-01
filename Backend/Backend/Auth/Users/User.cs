using Microsoft.AspNetCore.Identity;

namespace Backend.Auth.Users;

public class User : IdentityUser
{
    public bool HasChangedPassword { get; set; } = false;
}