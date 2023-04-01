namespace Backend.Features.Employees;

public class EmployeeRequest
{
    public string UserId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
    
    public string Position { get; set; } = string.Empty;

    public DateTime StartedWorking { get; set; }
}