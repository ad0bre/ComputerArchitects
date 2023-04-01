using Backend.Base.Models;

namespace Backend.Features.NewEmployee;

public class NewEmployee : Model
{
    public string UserId { get; set; } = string.Empty;
    
    public string Name { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Position { get; set; } = string.Empty;

    public DateTime StartedWorking { get; set; }

    public string BuddyId { get; set; } = string.Empty;
}