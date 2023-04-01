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

}