using System.ComponentModel.DataAnnotations;

namespace Backend.Features.Employees;

public class EmployeeRestrictedRequest
{
    [Required]public string Name { get; set; } = string.Empty;
    
    [Required]public string Email { get; set; } = string.Empty;

    public string? Bio { get; set; } = string.Empty;
}