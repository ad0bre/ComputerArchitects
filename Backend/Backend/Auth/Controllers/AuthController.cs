using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Auth.AuthViews;
using Backend.Auth.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Backend.Auth.Controllers;
[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    public AuthController(IConfiguration configuration, UserManager<User> userManager)  
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized("Email not found");
        }
        
        var wrongPass = !await _userManager.CheckPasswordAsync(user, request.Password);
        if (wrongPass)
        {
            return Unauthorized("Password not found");
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        
        var authClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new("id", user.Id),
            new("email", user.Email)
        };
        
        authClaims.AddRange(userRoles.Select(role => new Claim("roles", role)));
        
        var authSigninKey =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            expires: DateTime.UtcNow.AddMinutes(10),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
        );

        return Ok(new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }

    // [HttpPost]
    // [ProducesResponseType(StatusCodes.Status201Created)]
    // [ProducesResponseType(StatusCodes.Status400BadRequest)]
    // public async Task<ActionResult<UserView>> Register(RegisterView request)
    // {
    //     
    // }
    //
    // private async Task<ActionResult<UserView>> AddUser(RegisterView request, bool makeManager)
    // {
    //     var newUser = new User { Email = request.Email };
    //
    //     var result = await _userManager.CreateAsync(newUser, request.Password);
    //
    //     if (!result.Succeeded)
    //     {
    //         return NoContent();
    //     }
    //
    //     var user = await _userManager.FindByEmailAsync(newUser.Email);
    //     if (user is null)
    //     {
    //         return NotFound();
    //     }
    //     if (makeManager)
    //     {
    //         await _userManager.AddToRoleAsync(user, "manager");
    //     }
    //
    //     var roleName = (await _userManager.GetRolesAsync(user)).First();
    //     var role = await _roleManager.FindByNameAsync(roleName);
    //     if (role is null)
    //     {
    //         return NotFound();
    //     }
    //     return Created("", new UserView
    //     {
    //         Id = user.Id,
    //         Email = user.Email,
    //         RoleId = role.Id
    //     });
    // }
}