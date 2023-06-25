using CT.Examples.AspNetCoreWithAngular.Domain.Configuration;
using CT.Examples.AspNetCoreWithAngular.Entities;
using CT.Examples.AspNetCoreWithAngular.Models;
using CT.Examples.AspNetCoreWithAngular.Models.Input;
using CT.Examples.AspNetCoreWithAngular.Models.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CT.Examples.AspNetCoreWithAngular.Domain.Services;

public interface IAuthenticationService
{
    Task<OperationResult<JwtDto>> SignIn(SignInDto signInDto);
    Task<OperationResult<JwtDto>> SignUp(SignUpDto signUpDto);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly SigningCredentials _signingCredentials;
    private readonly AspNetCoreWithAngularContext _context;
    private readonly IPasswordHasher<Account> _passwordHasher;

    public AuthenticationService(AuthenticationOptions authenticationOptions, AspNetCoreWithAngularContext context,
        IPasswordHasher<Account> passwordHasher)
    {
        _signingCredentials = new SigningCredentials(authenticationOptions.Key, SecurityAlgorithms.HmacSha256);
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<OperationResult<JwtDto>> SignIn(SignInDto signInDto)
    {
        var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Email == signInDto.Email);

        if (account == null || !account.Enabled)
        {
            return OperationResultStatus.NotFound;
        }

        var result = _passwordHasher.VerifyHashedPassword(account, account.PasswordHash, signInDto.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return OperationResultStatus.BadRequest;
        }

        return GenerateJwt(account);
    }

    public async Task<OperationResult<JwtDto>> SignUp(SignUpDto signUpDto)
    {
        if (signUpDto.Password != signUpDto.RepeatPassword)
        {
            return OperationResultStatus.BadRequest;
        }

        var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Email == signUpDto.Email);

        if (account != null)
        {
            return OperationResultStatus.Conflict;
        }

        var firstUser = !await _context.Accounts.AnyAsync();

        account = new()
        {
            FirstName = signUpDto.FirstName,
            LastName = signUpDto.LastName,
            Email = signUpDto.Email,
            PasswordHash = _passwordHasher.HashPassword(account, signUpDto.Password),
            PhoneNumber = signUpDto.PhoneNumber,
            Role = firstUser ? WebRole.Admin : WebRole.User
        };

        _context.Accounts.Add(account);
        await _context.SaveChangesAsync();

        var jwt = GenerateJwt(account);

        return jwt;
    }

    private JwtDto GenerateJwt(Account account)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            new Claim(ClaimTypes.GivenName, account.FirstName),
            new Claim(ClaimTypes.Surname, account.LastName),
            new Claim(ClaimTypes.Email, account.Email),
            new Claim(ClaimTypes.Role, account.Role.ToString())
        };

        var token = new JwtSecurityToken("Portal", "Portal", claims, expires: DateTime.Now.AddDays(30), signingCredentials: _signingCredentials);

        return new() { Token = new JwtSecurityTokenHandler().WriteToken(token) };
    }
}
