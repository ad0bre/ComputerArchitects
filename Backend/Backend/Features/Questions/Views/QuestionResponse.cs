using Backend.Features.Questions.Enums;

namespace Backend.Features.Questions.Views;

public class QuestionResponse
{
    public string Id { get; set; }
    
    public string Title { get; set; } = string.Empty;

    public bool IsRequired { get; set; } = false;
    
    public string? Placeholder { get; set; }
    
    public QuestionType Type { get; set; }
    
    public FormType Form { get; set; }
}