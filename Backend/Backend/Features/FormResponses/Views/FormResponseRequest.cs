using System.ComponentModel.DataAnnotations;

namespace Backend.Features.FormResponses.Views;

public class FormResponseRequest
{
    [Required]public string UserId { get; set; } = string.Empty;
    
    [Required]public string QuestionId { get; set; } = string.Empty;
    
    [Required]public string Value { get; set; } = string.Empty;
}