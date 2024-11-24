using WebApplication2.DTO;

namespace WebApplication2.AccountServices
{
    public interface ICalculateBalance
    {
        Task<List<ReturnAccountDto>> CalTotalBalance();
    }
}