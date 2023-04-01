using System.ComponentModel.DataAnnotations;

namespace Backend.Features.QuestionOptions.Views;

public class QuestionOptionView
{
    [Required]public string QuestionId { get; set; } = string.Empty;
    
    [Required]public string Label { get; set; } = string.Empty;
}