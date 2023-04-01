using System.Net.Mime;
using Backend.Database;
using Backend.Features.QuestionOptions.Views;
using Backend.Features.Questions;
using Backend.Features.Questions.Views;
using Backend.Utils.AdminRoute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.QuestionOptions;

[ApiController]
[Route("api/questionoptions")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class QuestionOptionsController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public QuestionOptionsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<QuestionOptionView>> Post([FromBody] QuestionOptionView request)
    {
        var questionoption = new QuestionOption
        {
            QuestionId = request.QuestionId,
            Label = request.Label
        };

        var result = await _dbContext.Options.AddAsync(questionoption);
        await _dbContext.SaveChangesAsync();
        return Created($"options/{questionoption.Id}", new QuestionOptionView
        {
            QuestionId = questionoption.QuestionId,
            Label = questionoption.Label
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<QuestionOptionView>>> Get()
    {
        return Ok(await _dbContext.Options.Select(
            option => new QuestionOptionView
            {
                QuestionId = option.QuestionId,
                Label = option.Label
            }).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionOptionView>> Get([FromRoute] string id)
    {
        var option = await _dbContext.Options.FirstOrDefaultAsync(e => e.Id == id);
        if (option is null)
        {
            return NotFound("Option not found");
        }

        return Ok(new QuestionOptionView
        {
            QuestionId = option.QuestionId,
            Label = option.Label
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<QuestionOption>> Delete([FromRoute] string id)
    {
        var option = await _dbContext.Options.FirstOrDefaultAsync(e => e.Id == id);
        if (option is null)
        {
            return NotFound("Option not found");
        }

        var result = _dbContext.Options.Remove(option);
        await _dbContext.SaveChangesAsync();

        return Ok(option);
    }


}