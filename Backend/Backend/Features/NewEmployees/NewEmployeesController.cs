using System.Net.Mime;
using Backend.Auth.Users;
using Backend.Database;
using Backend.Features.Employees;
using Backend.Features.FormResponses;
using Backend.Features.NewEmployees.Views;
using Backend.Features.OldEmployees;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.NewEmployees;

[ApiController]
[Route("api/newemployees")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class NewEmployeesController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly UserManager<User> _users;

    public NewEmployeesController(AppDbContext dbContext, UserManager<User> users)
    {
        _dbContext = dbContext;
        _users = users;
    }

    [HttpPost]
    // [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NewEmployeeResponse>> Post([FromBody] EmployeeRequest request)
    {
        var user = await _users.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var newEmployee = new NewEmployee
        {
            UserId = user.Id,
            Bio = request.Bio,
            Email = user.Email,
            Name = request.Name,
            Position = request.Position,
            StartedWorking = request.StartedWorking,
            BuddyId = string.Empty
        };

        var result = await _dbContext.NewEmployees.AddAsync(newEmployee);
        await _dbContext.SaveChangesAsync();
        
        return Created($"newemployees/{newEmployee.Id}", new NewEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            BuddyId = result.Entity.BuddyId
        });
    }

    [HttpGet("buddyid/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<NewEmployeeResponse>>> GetbyBuddyId([FromRoute] string id)
    {
        var buddy = await _dbContext.OldEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (buddy is null)
        {
            return NotFound("Buddy not found");
        }

        return Ok(await _dbContext.NewEmployees
            .Where(n => n.BuddyId == buddy.Id)
            .Select(newEmployee => new NewEmployeeResponse
            {
                Id = newEmployee.Id,
                UserId = newEmployee.UserId,
                Name = newEmployee.Name,
                Bio = newEmployee.Bio,
                Email = newEmployee.Email,
                Position = newEmployee.Position,
                StartedWorking = newEmployee.StartedWorking,
                BuddyId = newEmployee.BuddyId
            }).ToListAsync());
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<NewEmployeeResponse>>> Get()
    {
        return Ok(await _dbContext.NewEmployees.Select(
            newEmployee => new NewEmployeeResponse
            {
                Id = newEmployee.Id,
                UserId = newEmployee.UserId,
                Name = newEmployee.Name,
                Bio = newEmployee.Bio,
                Email = newEmployee.Email,
                Position = newEmployee.Position,
                StartedWorking = newEmployee.StartedWorking,
                BuddyId = newEmployee.BuddyId
            }).ToListAsync());
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NewEmployeeResponse>> Get([FromRoute] string id)
    {
        var newEmployee = await _dbContext.NewEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (newEmployee is null)
        {
            return NotFound("Employee not found");
        }

        return Ok(new NewEmployeeResponse
        {
            Id = newEmployee.Id,
            UserId = newEmployee.UserId,
            Name = newEmployee.Name,
            Bio = newEmployee.Bio,
            Email = newEmployee.Email,
            Position = newEmployee.Position,
            StartedWorking = newEmployee.StartedWorking,
            BuddyId = newEmployee.BuddyId
        });
    }
    
    [HttpGet("userid/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<NewEmployeeResponse>> GetbyUserId([FromRoute] string id)
    {
        var user = await _users.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound("User not found");
        }

        var newEmployee = await _dbContext.NewEmployees.FirstOrDefaultAsync(e => e.UserId == user.Id);
        if (newEmployee is null)
        {
            return NotFound("Employee not found");
        }

        return Ok(new NewEmployeeResponse
        {
            Id = newEmployee.Id,
            UserId = newEmployee.UserId,
            Name = newEmployee.Name,
            Bio = newEmployee.Bio,
            Email = newEmployee.Email,
            Position = newEmployee.Position,
            StartedWorking = newEmployee.StartedWorking,
            BuddyId = newEmployee.BuddyId
        });
    }
    
    [HttpDelete("{id}")]
    // [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NewEmployeeResponse>> Delete([FromRoute] string id)
    {
        var oldEmployee = await _dbContext.NewEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (oldEmployee is null)
        {
            return NotFound("Employee not found");
        }

        var result = _dbContext.NewEmployees.Remove(oldEmployee);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new NewEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking, 
            BuddyId = result.Entity.BuddyId
        });
    }
    
    [HttpPatch("{id}")]
    // [Authorize(Roles = ManagerRole.Manager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NewEmployeeResponse>> Update([FromRoute] string id, [FromBody] EmployeeRequest request)
    {
        var newEmployee = await _dbContext.NewEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (newEmployee is null)
        {
            return NotFound("Employee not found");
        }
        
        var user = await _users.FindByIdAsync(request.UserId);
        if (user is null)
        {
            return NotFound("User not found");
        } 

        newEmployee.UserId = request.UserId != string.Empty ? request.UserId : newEmployee.UserId;
        newEmployee.Name = request.Name != string.Empty ? request.Name : newEmployee.Name;
        newEmployee.Bio = request.Bio != string.Empty ? request.Bio : newEmployee.Bio;
        newEmployee.Position = request.Position != string.Empty ? request.Position : newEmployee.Position;
        newEmployee.StartedWorking = request.StartedWorking != null ? request.StartedWorking : newEmployee.StartedWorking;
        newEmployee.Email = user.Email;
        newEmployee.Updated = DateTime.UtcNow;

        var result = _dbContext.NewEmployees.Update(newEmployee);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new NewEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            BuddyId = result.Entity.BuddyId
        });
    }
    
    [HttpPatch("employee/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NewEmployeeResponse>> RestrictedUpdate([FromRoute] string id, [FromBody] EmployeeRestrictedRequest request)
    {
        var newEmployee = await _dbContext.NewEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (newEmployee is null)
        {
            return NotFound("Employee not found");
        }
        
        var user = await _users.FindByIdAsync(newEmployee.UserId);
        if (user is null)
        {
            return NotFound("User not found");
        } 
        
        newEmployee.Name = request.Name != string.Empty ? request.Name : newEmployee.Name;
        newEmployee.Bio = request.Bio != string.Empty ? request.Bio : newEmployee.Bio;
        newEmployee.Email = user.Email;
        newEmployee.Updated = DateTime.UtcNow;

        var result = _dbContext.NewEmployees.Update(newEmployee);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new NewEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            BuddyId = result.Entity.BuddyId
        });
    }
    
    [HttpPatch("assignbuddy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> AssignBuddy([FromBody] string id)
    {
        var newEmployees = _dbContext.Set<NewEmployee>();
        var oldEmployees = _dbContext.Set<OldEmployee>();
        var responses = _dbContext.Set<FormResponse>();

        var newEmployee = await newEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (newEmployee is null)
        {
            return NotFound("New employee not found");
        }
        
        OldEmployee bestMatch = null;
        var maxCommon = -1;
        
        var newEmployeeTechnologies = await responses
            .Where(r => r.UserId == newEmployee.UserId)
            .Select(r => r.Value)
            .ToListAsync();
        
        foreach (var oldEmployee in oldEmployees)
        {
            if(!oldEmployee.IsAccepting) continue;
            var oldEmployeeTechnologies = await responses
                .Where(r => r.UserId == oldEmployee.UserId)
                .Select(r => r.Value)
                .ToListAsync();
            
            var commonCount = newEmployeeTechnologies.Intersect(oldEmployeeTechnologies).Count();

            if (commonCount <= maxCommon) continue;
            maxCommon = commonCount;
            bestMatch = oldEmployee;
        }

        if (bestMatch is null)
        {
            return NotFound("Could not find a workplace buddy");
        }
        
        newEmployee.BuddyId = bestMatch.Id;
        var result = newEmployees.Update(newEmployee);
        if (result.State is not EntityState.Modified)
        {
            return BadRequest("Could not update new employee model");
        }
        await _dbContext.SaveChangesAsync();
        return Ok(newEmployee.BuddyId);
    }

    [HttpPatch("changebuddy/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NewEmployeeResponse>> ChangeBuddy([FromRoute] string id, [FromBody] string buddyId)
    {
        var newEmployee = await _dbContext.NewEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (newEmployee is null)
        {
            return NotFound("New employee not found");
        }

        var buddy = await _dbContext.OldEmployees.FirstOrDefaultAsync(e => e.Id == id);
        if (buddy is null)
        {
            return NotFound("Buddy not found");
        }

        newEmployee.BuddyId = buddy.Id;
        var result = _dbContext.NewEmployees.Update(newEmployee);
        await _dbContext.SaveChangesAsync();
        
        return Ok(new NewEmployeeResponse
        {
            Id = result.Entity.Id,
            UserId = result.Entity.UserId,
            Name = result.Entity.Name,
            Bio = result.Entity.Bio,
            Email = result.Entity.Email,
            Position = result.Entity.Position,
            StartedWorking = result.Entity.StartedWorking,
            BuddyId = result.Entity.BuddyId
        });
    }
}