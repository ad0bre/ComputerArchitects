using Backend.Base.Models;

namespace Backend.Features.FormResponses;

public class FormResponse : Model
{
    public string UserId { get; set; } = string.Empty;
    
    public string QuestionId { get; set; } = string.Empty;
    
    public string Value { get; set; } = string.Empty;
}