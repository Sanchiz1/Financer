using Domain.ValueObjects;
using Infrastructure.Errors;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using SharedKernel.Result;

namespace Infrastructure.Identity;
public class IdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly TokenService _tokenService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    public async Task<Result> RegisterAsync(string firstName, string lastName, string email, Currency preferredCurrency, string password)
    {
        var user = new ApplicationUser
        {
            FirstName = new Name(firstName),
            LastName = new Name(lastName),
            PrefferedCurrency = preferredCurrency,
            UserName = email,
            Email = email,
        };

        var result = await _userManager.CreateAsync(user, password);
        
        return MapIdentityResult(result);
    }

    public async Task<Result<string>> LoginAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null) return Result.Failure<string>(IdentityErrors.UserNotFound);

        await _signInManager.SignInAsync(user, true);

        var token = _tokenService.GenerateToken(user);

        return token;
    }

    public async Task<Result<string>> LoginPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null ||  !await _userManager.CheckPasswordAsync(user, password)) 
            return Result.Failure<string>(IdentityErrors.InvalidPasswordOrUsername);

        await _signInManager.SignInAsync(user, true);

        var token = _tokenService.GenerateToken(user);

        return token;
    }

    public async Task<Result> UpdatePreferredCurrency(Guid userId, Currency currency)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is null) return Result.Failure(IdentityErrors.UserNotFound);

        user.PrefferedCurrency = currency;

        var result = await _userManager.UpdateAsync(user);

        return MapIdentityResult(result);
    }


    public async Task<Result> DeleteUserAsync(Guid userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        if (user is null) return Result.Failure<string>(IdentityErrors.UserNotFound);

        var result = await _userManager.DeleteAsync(user);

        return MapIdentityResult(result);
    }

    private static Result MapIdentityResult(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            return Result.Failure(result.Errors.First().ToResultError());
        }

        return Result.Success();
    }
}

