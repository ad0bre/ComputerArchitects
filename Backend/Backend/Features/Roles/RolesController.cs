using System.Net.Mime;
using System.Web.Http;
using Backend.Auth.Users;
using Backend.Features.Roles.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Features.Roles;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/roles")]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class RolesController : ControllerBase
{
    private readonly RoleManager<Role> _roles;

    public RolesController(RoleManager<Role> roles)
    {
        _roles = roles;
    }

    [Microsoft.AspNetCore.Mvc.HttpGet]
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

    [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
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

    [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
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
}