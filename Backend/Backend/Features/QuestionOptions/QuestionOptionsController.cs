using System.Net.Mime;
using Backend.Database;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Features.QuestionOptions;

[ApiController]
[Route("api/questionoptions")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class QuestionOptionsController : ControllerBase
{
    // private readonly AppDbContext _dbContext;
}