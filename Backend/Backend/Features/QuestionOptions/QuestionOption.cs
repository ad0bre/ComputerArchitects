using Backend.Base.Models;

namespace Backend.Features.QuestionOptions;

public class QuestionOption : Model
{
    public string QuestionId { get; set; } = string.Empty;
    
    public string Label { get; set; } = string.Empty;
}