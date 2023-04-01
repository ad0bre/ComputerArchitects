using System.Net.Mime;
using Backend.Database;
using Backend.Features.FormResponses.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.FormResponses;

[ApiController]
[Route("api/formresponses")]
[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
public class FormResponsesController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public FormResponsesController (AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<FormResponseView>> Post([FromBody] FormResponseRequest request)
    {
        var response = new FormResponse
        {
            QuestionId = request.QuestionId,
            UserId = request.QuestionId,
            Value = request.Value
        };

        var result = await _dbContext.Responses.AddAsync(response);
        await _dbContext.SaveChangesAsync();

        return Created($"responses/{response.Id}", new FormResponseView
        {
            Id = response.Id,
            QuestionId = response.QuestionId,
            UserId = response.UserId,
            Value = response.Value
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FormResponseView>>> Get()
    {
        return Ok(await _dbContext.Responses.Select(
            response => new FormResponseView
            {
                Id = response.Id,
                QuestionId = response.QuestionId,
                UserId = response.UserId,
                Value = response.Value 
            }
        ).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormResponseView>> Get([FromRoute] string id)
    {
        var response = await _dbContext.Responses.FirstOrDefaultAsync(e => e.Id == id);
        if (response is null)
        {
            return NotFound("Response not found");
        }

        return Ok(new FormResponseView
        {
            Id = response.Id,
            QuestionId = response.QuestionId,
            UserId = response.UserId,
            Value = response.Value
        });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FormResponseView>> Delete([FromRoute] string id)
    {
        var response = await _dbContext.Responses.FirstOrDefaultAsync(e => e.Id == id);
        if (response is null)
        {
            return NotFound("Response not found");
        }

        var result = _dbContext.Responses.Remove(response);
        await _dbContext.SaveChangesAsync();

        return Ok(new FormResponseView
        {
            Id = result.Entity.Id,
            QuestionId = result.Entity.QuestionId,
            UserId = result.Entity.UserId,
            Value = result.Entity.Value
        });
    }
}