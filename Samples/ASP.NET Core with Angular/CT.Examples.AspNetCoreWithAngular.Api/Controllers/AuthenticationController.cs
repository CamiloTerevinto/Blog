using CT.Examples.AspNetCoreWithAngular.Api.Middleware;
using CT.Examples.AspNetCoreWithAngular.Domain.Services;
using CT.Examples.AspNetCoreWithAngular.Models.Input;
using CT.Examples.AspNetCoreWithAngular.Models.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CT.Examples.AspNetCoreWithAngular.Api.Controllers;

[Route("api/authentication")]
[AllowAnonymous]
public class AuthenticationController : BaseApiController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<JwtDto>> Authenticate([FromBody] SignInDto signInDto)
    {
        return await _authenticationService.SignIn(signInDto).ToActionResult();
    }

    [HttpPost("sign-up")]
    public async Task<ActionResult<JwtDto>> SignUp([FromBody] SignUpDto signUpDto)
    {
        return await _authenticationService.SignUp(signUpDto).ToActionResult();
    }
}
