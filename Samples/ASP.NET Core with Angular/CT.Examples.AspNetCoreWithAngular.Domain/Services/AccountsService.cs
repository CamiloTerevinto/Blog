using CT.Examples.AspNetCoreWithAngular.Entities;
using CT.Examples.AspNetCoreWithAngular.Models;
using CT.Examples.AspNetCoreWithAngular.Models.Input;
using CT.Examples.AspNetCoreWithAngular.Models.Output;
using Microsoft.EntityFrameworkCore;

namespace CT.Examples.AspNetCoreWithAngular.Domain.Services;

public interface IAccountsService
{
    Task<OperationResult<AccountDto>> GetAccountAsync(int accountId);
    Task<OperationResult<IEnumerable<AccountDto>>> GetAccountsAsync();
    Task<OperationResultStatus> UpdateAccountAsync(int accountId, UpdateAccountDto updateAccountDto);
}

public class AccountsService : IAccountsService
{
    private readonly AspNetCoreWithAngularContext _context;

    public AccountsService(AspNetCoreWithAngularContext context)
    {
        _context = context;
    }

    public async Task<OperationResult<IEnumerable<AccountDto>>> GetAccountsAsync()
    {
        return await _context.Accounts
            .Select(u => new AccountDto
            {
                AccountId = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
            })
            .ToArrayAsync();
    }

    public async Task<OperationResult<AccountDto>> GetAccountAsync(int accountId)
    {
        var account = await _context.Accounts.FindAsync(accountId);

        return new AccountDto
        {
            AccountId = account.Id,
            Email = account.Email,
            FirstName = account.FirstName,
            LastName = account.LastName,
            PhoneNumber = account.PhoneNumber,
        };
    }

    public async Task<OperationResultStatus> UpdateAccountAsync(int accountId, UpdateAccountDto updateAccountAccountDto)
    {
        var currentAccount = await _context.Accounts.FindAsync(accountId);

        if (currentAccount == null)
        {
            return OperationResultStatus.NotFound;
        }

        currentAccount.Email = updateAccountAccountDto.Email;
        currentAccount.PhoneNumber = updateAccountAccountDto.PhoneNumber;
        currentAccount.FirstName = updateAccountAccountDto.FirstName;
        currentAccount.LastName = updateAccountAccountDto.LastName;

        await _context.SaveChangesAsync();

        return OperationResultStatus.Success;
    }
}
