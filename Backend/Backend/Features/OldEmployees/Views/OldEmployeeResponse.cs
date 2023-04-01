namespace Backend.Features.OldEmployees.Views;

public class OldEmployeeResponse
{
    public string Id { get; set; } = string.Empty;
    
    public string UserId { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
    
    public string Position { get; set; } = string.Empty;

    public DateTime StartedWorking { get; set; }

    public bool IsAccepting { get; set; } = true;
}