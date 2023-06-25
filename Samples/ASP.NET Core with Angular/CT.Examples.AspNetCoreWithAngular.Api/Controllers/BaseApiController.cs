using CT.Examples.AspNetCoreWithAngular.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CT.Examples.AspNetCoreWithAngular.Api.Controllers;

[Produces("application/json")]
[ApiController]
public class BaseApiController : ControllerBase
{
    public int AccountId => int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    public bool IsCurrentAccountAdmin => HttpContext.User.FindFirst(ClaimTypes.Role)!.Value == WebRole.Admin.ToString();
}