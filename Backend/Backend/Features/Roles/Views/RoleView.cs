using System.ComponentModel.DataAnnotations;

namespace Backend.Features.Roles.Views;

public class RoleView
{
    public string Id { get; set; }
    
    [Required]public string Name { get; set; }
}