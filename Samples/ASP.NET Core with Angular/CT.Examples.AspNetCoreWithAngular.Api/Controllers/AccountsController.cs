using CT.Examples.AspNetCoreWithAngular.Api.Middleware;
using CT.Examples.AspNetCoreWithAngular.Domain.Services;
using CT.Examples.AspNetCoreWithAngular.Models.Input;
using CT.Examples.AspNetCoreWithAngular.Models.Output;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CT.Examples.AspNetCoreWithAngular.Api.Controllers;

[Route("api/accounts")]
public class AccountsController : BaseApiController
{
    private readonly IAccountsService _service;

    public AccountsController(IAccountsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
    {
        if (!IsCurrentAccountAdmin)
        {
            return Forbid();
        }

        return await _service.GetAccountsAsync().ToActionResult();
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateAccount(int userId, [Required] UpdateAccountDto updateAccountAccountDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        else if (!IsCurrentAccountAdmin)
        {
            return Forbid();
        }

        return await _service.UpdateAccountAsync(userId, updateAccountAccountDto).ToActionResult();
    }
}
