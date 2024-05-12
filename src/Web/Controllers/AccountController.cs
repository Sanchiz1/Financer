using Domain.ValueObjects;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Infrastructure;
using Web.Models.Identity;

namespace Web.Controllers;

public class AccountController : BaseApiController
{

    private readonly IdentityService _identityService;
    public AccountController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] RegisterDto request)
    {
        var result = await _identityService.RegisterAsync(
            request.FirstName,
            request.LastName,
            request.Email,
            request.PreferredCurrency, 
            request.Password);

        return this.HandleResult(result);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto request)
    {
        var result = await _identityService.LoginPasswordAsync(request.Email, request.Password);

        return this.HandleResult(result);
    }

    [HttpPatch]
    [Route("Currency")]
    public async Task<ActionResult<string>> UpdatePreferredCurrency([FromBody] Currency currency)
    {
        var userId = User.GetUserId();

        var result = await _identityService.UpdatePreferredCurrency(userId, currency);

        return this.HandleResult(result);
    }

    [HttpDelete]
    public async Task<ActionResult<string>> DeleteUser()
    {
        var userId = User.GetUserId();

        var result = await _identityService.DeleteUserAsync(userId);

        return this.HandleResult(result);
    }
}
