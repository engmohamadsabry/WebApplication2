using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.DTO;
using WebApplication2.Models;

namespace WebApplication2.AccountServices
{
    public class AccountDetails : IAccountDetails
    {
        private readonly ApplicationDbContext _context;
        public AccountDetails(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<DetailDto>> GetDetails(string accountId)
        {
            List<DetailDto> detailDtos = new();
            var account = await _context.Accounts.FirstOrDefaultAsync(x => x.AccNumber == accountId);
            if (account == null) { return detailDtos; }
            var children = await _context.Accounts
                           .Where(a => a.AccParent == account.AccNumber).ToListAsync();

            foreach (var child in children)
            {
                DetailDto dto = new DetailDto();
                dto.Track.Add(account.AccNumber);
                await ChildRecursive(child, dto);
                detailDtos.Add(dto);
            }
            return detailDtos;
            async Task<DetailDto> ChildRecursive(Account acc, DetailDto dto)
            {
                var child = await _context.Accounts
                            .FirstOrDefaultAsync(a => a.AccParent == acc.AccNumber);
                dto.Track.Add(acc.AccNumber);
                if (child == null)
                {
                    dto.Balance = acc.Balance;
                    return dto;
                }
                return await ChildRecursive(child, dto);
            }
        }
    }
}
