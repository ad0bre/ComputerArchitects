namespace Backend.Auth.AuthViews;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    
    public DateTime Expiration { get; set; }
}