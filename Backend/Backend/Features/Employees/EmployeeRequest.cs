using System.ComponentModel.DataAnnotations;

namespace Backend.Features.Employees;

public class EmployeeRequest
{
    [Required]public string UserId { get; set; }
    
    [Required]public string Name { get; set; } = string.Empty;
    
    [Required]public string Email { get; set; } = string.Empty;

    public string? Bio { get; set; } = string.Empty;
    
    [Required]public string Position { get; set; } = string.Empty;

    [Required]public DateTime StartedWorking { get; set; }
}