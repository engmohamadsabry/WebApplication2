using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.AccountServices
{
    public class CalculateBalance : ICalculateBalance
    {
        private readonly ApplicationDbContext _context;

        public CalculateBalance(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<ReturnAccountDto>> CalTotalBalance()
        {
            var accounts = await _context.Accounts.ToListAsync();
            List<ReturnAccountDto> returnAccountDtos = new List<ReturnAccountDto>();
            foreach (var account in accounts)
            {
                if (account.AccParent == null)
                {
                    ReturnAccountDto dto = new ReturnAccountDto() { TopLevelAccount = account.AccNumber };
                    dto.TotalBalance = await SumBalancesRecursive(account);
                    returnAccountDtos.Add(dto);
                    async Task<decimal?> SumBalancesRecursive(Account account)
                    {
                        // Retrieve all children of the current account
                        var children = await _context.Accounts
                            .Where(a => a.AccParent == account.AccNumber)
                            .ToListAsync();
                        decimal totalBalance = 0;
                        // If there are no children, return 0
                        if (!children.Any())
                        {
                            return totalBalance = account.Balance.HasValue ? account.Balance.Value : 0;
                        }

                        // Sum the current account's balance (if it has a balance)
                        if (account.Balance.HasValue)
                        {
                            totalBalance += account.Balance.Value;
                        }

                        // Recursively sum the balances of child accounts
                        foreach (var child in children)
                        {
                            totalBalance += (await SumBalancesRecursive(child)) ?? 0;
                        }

                        return totalBalance;
                    }
                }
            }
            return returnAccountDtos;
        }
    }
}
