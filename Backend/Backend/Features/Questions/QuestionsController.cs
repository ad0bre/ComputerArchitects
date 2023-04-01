using System.Net.Mime;
using Backend.Database;
using Backend.Features.Questions.Views;
using Backend.Utils.AdminRoute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Questions;

[ApiController]
[Route("api/questions")]
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
    [Authorize(Roles = ManagerRole.Manager)]
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
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<QuestionResponse>>> Get()
    {
        return Ok(await _dbContext.Questions.Select(
            question => new QuestionResponse
            {
                Id = question.Id,
                Title = question.Title,
                IsRequired = question.IsRequired,
                Placeholder = question.Placeholder,
                Type = question.Type,
                Form = question.Form
            }).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionResponse>> Get([FromRoute] string id)
    {
        var question = await _dbContext.Questions.FirstOrDefaultAsync(e => e.Id == id);
        if (question is null)
        {
            return NotFound("Question not found");
        }

        return Ok(new QuestionResponse
        {
            Id = question.Id,
            Title = question.Title,
            IsRequired = question.IsRequired,
            Placeholder = question.Placeholder,
            Type = question.Type,
            Form = question.Form
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionResponse>> Delete([FromRoute] string id)
    {
        var question = await _dbContext.Questions.FirstOrDefaultAsync(e => e.Id == id);
        if (question is null)
        {
            return NotFound("Question not found");
        }

        var result = _dbContext.Questions.Remove(question);
        await _dbContext.SaveChangesAsync();
        return Ok(new QuestionResponse
        {
            Id = result.Entity.Id,
            Title = result.Entity.Title,
            IsRequired = result.Entity.IsRequired,
            Placeholder = result.Entity.Placeholder,
            Type = result.Entity.Type,
            Form = result.Entity.Form
        });
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionResponse>> Update([FromRoute] string id, [FromBody] QuestionRequest request)
    {
        var question = await _dbContext.Questions.FirstOrDefaultAsync(e => e.Id == id);
        if (question is null)
        {
            return NotFound("Question not found");
        }

        question.Title = request.Title == string.Empty ? question.Title : request.Title;
        question.IsRequired = request.IsRequired;
        question.Placeholder = request.Placeholder == string.Empty ? question.Placeholder : request.Placeholder;
        question.Form = request.Form;
        question.Type = request.Type;
        question.Updated = DateTime.UtcNow;

        var result = _dbContext.Questions.Update(question);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new QuestionResponse
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