using System.Net.Mime;
using Backend.Database;
using Backend.Features.Questions.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.Questions;

[ApiController]
[Route("api/questions")]
[Authorize(Roles = "manager")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class QuestionsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public QuestionsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<QuestionResponse>> Post([FromBody] QuestionRequest request)
    {
        var question = new QuestionModel
        {
            Title = request.Title,
            IsRequired = request.IsRequired,
            Placeholder = request.Placeholder,
            Type = request.Type,
            Form = request.Form
        };

        var result = await _dbContext.AddAsync(question);
        await _dbContext.SaveChangesAsync();

        return Created($"questions/{question.Id}", new QuestionResponse
        {
            Id = result.Entity.Id,
            Title = result.Entity.Title,
            IsRequired = result.Entity.IsRequired,
            Placeholder = result.Entity.Placeholder,
            Type = result.Entity.Type,
            Form = result.Entity.Form
        });
    }
}