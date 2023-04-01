namespace Backend.Features.Employees;

public class EmployeeRestrictedRequest
{
    public string Name { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
}