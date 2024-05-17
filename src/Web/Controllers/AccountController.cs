using Domain.ValueObjects;
using Infrastructure.Errors;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web.Infrastructure;
using Web.Models.Identity;

namespace Web.Controllers;
[Authorize]
public class AccountsController(
    UserManager<ApplicationUser> userManager,
    TokenService tokenService,
    IConfiguration configuration)
    : BaseApiController
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly TokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await this._userManager.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user is null)
        {
            return Unauthorized();
        }

        var result = await this._userManager.CheckPasswordAsync(user, loginDto.Password);

        if (result)
        {
            return Ok(new UserDto(this._tokenService.GenerateToken(user), user.UserName, user.PrefferedCurrency.Code));
        }

        return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (await this._userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email is already taken.");
            return ValidationProblem();
        }

        var user = new ApplicationUser
        {
            FirstName = new Name(registerDto.FirstName),
            LastName = new Name(registerDto.LastName),
            PrefferedCurrency = Currency.FromCode(registerDto.PreferredCurrency),
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await this._userManager.CreateAsync(user, registerDto.Password);

        return result.Succeeded
            ? Ok(new UserDto(this._tokenService.GenerateToken(user), user.UserName, user.PrefferedCurrency.Code))
            : BadRequest(result.Errors);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await this._userManager.Users
            .FirstOrDefaultAsync(u => u.Email == User.FindFirstValue(ClaimTypes.Email));

        return new UserDto(this._tokenService.GenerateToken(user), user.UserName, user.PrefferedCurrency.Code);
    }

    [HttpPatch]
    [Route("currency")]
    public async Task<IActionResult> UpdatePrefferedCurrency([FromBody] string currency)
    {
        var userId = this.User.GetUserId();

        var user = this._userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is null)
        {
            return NotFound(IdentityErrors.UserNotFound);
        }

        user.PrefferedCurrency = Currency.FromCode(currency);

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded
            ? Ok("Preferred currency was successfylly updated")
            : BadRequest(result.Errors);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteUser()
    {
        var userId = this.User.GetUserId();

        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is null)
        {
            return NotFound(IdentityErrors.UserNotFound);
        }

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded
            ? Ok("User was successfylly deleted")
            : BadRequest(result.Errors);
    }
}