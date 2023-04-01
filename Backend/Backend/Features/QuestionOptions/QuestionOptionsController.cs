using System.Net.Mime;
using Backend.Database;
using Backend.Features.Questions.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    
    // [HttpPost]
    // [Authorize(Roles = "manager")]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // public async Task<ActionResult<QuestionResponse>> Post([])

}