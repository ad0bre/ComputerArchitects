using Backend.Database;
using Backend.Features.FormResponses;
using Backend.Features.NewEmployees;
using Backend.Features.NewEmployees.Views;
using Backend.Features.OldEmployees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Pair;

[ApiController]
[Route("api/givemeabuddy")]
public class BuddyController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<NewEmployee> _newEmployees;
    private readonly DbSet<OldEmployee> _oldEmployees;
    private readonly DbSet<FormResponse> _responses;

    public BuddyController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
        _newEmployees = dbContext.Set<NewEmployee>();
        _oldEmployees = dbContext.Set<OldEmployee>();
        _responses = dbContext.Set<FormResponse>();
    }

    

}