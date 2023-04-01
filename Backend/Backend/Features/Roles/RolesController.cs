using System.Net.Mime;
using Backend.Auth.Users;
using Backend.Features.Roles.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Roles;

[ApiController]
[Route("api/roles")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class RolesController : ControllerBase
{
    private readonly RoleManager<Role> _roles;

    public RolesController(RoleManager<Role> roles)
    {
        _roles = roles;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoleView>>> Get()
    {
        return Ok(await _roles.Roles.Select(
                role => new RoleView
                {
                    Id = role.Id,
                    Name = role.Name
                }
            ).ToListAsync());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleView>> Get([FromRoute] string id)
    {
        var role = await _roles.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound("Role not found");
        }

        return Ok(new RoleView
        {
            Id = role.Id,
            Name = role.Name
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Role>> Delete([FromRoute] string id)
    {
        var role = await _roles.FindByIdAsync(id);
        if (role is null)
        {
            return NotFound("Role not found");
        }

        var result = await _roles.DeleteAsync(role);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(role);
    }

    [HttpPost]
    [Authorize(Roles = "manager")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleView>> Post([FromBody] RoleView request)
    {
        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            Name = request.Name
        };
        var result = await _roles.CreateAsync(role);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Created($"roles/{role.Id}", new RoleView
        {
            Id = role.Id,
            Name = role.Name
        });

    }
}