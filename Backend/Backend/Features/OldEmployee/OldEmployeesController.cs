using System.Net.Mime;
using Backend.Auth.Users;
using Backend.Database;
using Backend.Features.Employees;
using Backend.Features.OldEmployee.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.OldEmployee;

[ApiController]
[Route("api/oldemployees")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class OldEmployeesController : ControllerBase
{
    private readonly DbSet<OldEmployee> _employees;
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _userManager;

    public OldEmployeesController(AppDbContext dbContext, UserManager<User> userManager)
    {
        _employees = dbContext.Set<OldEmployee>();
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize(Roles = "manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OldEmployeeResponse>> Post([FromBody] EmployeeRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var oldEmployee = new OldEmployee
        {
            UserId = user.Id,
            Bio = request.Bio,
            Email = user.Email,
            Name = request.Name,
            Position = request.Position,
            StartedWorking = request.StartedWorking,
            IsAccepting = true
        };

        var result = await _employees.AddAsync(oldEmployee);
        await SaveChanges();
        return Created($"oldemployees/{oldEmployee.Id}", new OldEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            IsAccepting = result.Entity.IsAccepting
        });
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<OldEmployeeResponse>>> Get()
    {
        return Ok(await _employees.Select(
            oldEmployee => new OldEmployeeResponse
            {
                Id = oldEmployee.Id,
                UserId = oldEmployee.UserId,
                Name = oldEmployee.Name,
                Bio = oldEmployee.Bio,
                Email = oldEmployee.Email,
                Position = oldEmployee.Position,
                StartedWorking = oldEmployee.StartedWorking,
                IsAccepting = oldEmployee.IsAccepting
            }).ToListAsync());
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OldEmployeeResponse>> GetbyId([FromRoute] string id)
    {
        var oldEmployee = await _employees.FirstOrDefaultAsync(e => e.Id == id);
        if (oldEmployee is null)
        {
            return NotFound("Employee not found");
        }

        return Ok(new OldEmployeeResponse
        {
            Id = oldEmployee.Id,
            UserId = oldEmployee.UserId,
            Name = oldEmployee.Name,
            Bio = oldEmployee.Bio,
            Email = oldEmployee.Email,
            Position = oldEmployee.Position,
            StartedWorking = oldEmployee.StartedWorking,
            IsAccepting = oldEmployee.IsAccepting
        });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OldEmployeeResponse>> Delete([FromRoute] string id)
    {
        var oldEmployee = await _employees.FirstOrDefaultAsync(e => e.Id == id);
        if (oldEmployee is null)
        {
            return NotFound("Employee not found");
        }

        var result = _employees.Remove(oldEmployee);
        await SaveChanges();
        
        return Ok(new OldEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            IsAccepting = result.Entity.IsAccepting
        });
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OldEmployeeResponse>> Update([FromRoute] string id, [FromBody] EmployeeRequest request)
    {
        var oldEmployee = await _employees.FirstOrDefaultAsync(e => e.Id == id);
        if (oldEmployee is null)
        {
            return NotFound("Employee not found");
        }

        oldEmployee.UserId = request.UserId != string.Empty ? request.UserId : oldEmployee.UserId;
        oldEmployee.Name = request.Name != string.Empty ? request.Name : oldEmployee.Name;
        oldEmployee.Email = request.Email != string.Empty ? request.Email : oldEmployee.Email;
        oldEmployee.Bio = request.Bio != string.Empty ? request.Bio : oldEmployee.Bio;
        oldEmployee.Position = request.Position != string.Empty ? request.Position : oldEmployee.Position;
        oldEmployee.StartedWorking = request.StartedWorking != null ? request.StartedWorking : oldEmployee.StartedWorking;
        oldEmployee.Updated = DateTime.UtcNow;

        var result = _employees.Update(oldEmployee);
        await SaveChanges();
        
        return Ok(new OldEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            IsAccepting = result.Entity.IsAccepting
        });
    }

    [HttpPatch("employee/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OldEmployeeResponse>> RestrictedUpdate([FromRoute] string id, [FromBody] EmployeeRestrictedRequest request)
    {
        var oldEmployee = await _employees.FirstOrDefaultAsync(e => e.Id == id);
        if (oldEmployee is null)
        {
            return NotFound("Employee not found");
        }

        oldEmployee.Name = request.Name != string.Empty ? request.Name : oldEmployee.Name;
        oldEmployee.Email = request.Email != string.Empty ? request.Email : oldEmployee.Email;
        oldEmployee.Bio = request.Bio != string.Empty ? request.Bio : oldEmployee.Bio;
        oldEmployee.Updated = DateTime.UtcNow;

        var result = _employees.Update(oldEmployee);
        await SaveChanges();
        
        return Ok(new OldEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            IsAccepting = result.Entity.IsAccepting
        });
    }
    private async Task SaveChanges()
    {
        await _dbContext.SaveChangesAsync();
    }
}