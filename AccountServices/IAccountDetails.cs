using WebApplication2.DTO;

namespace WebApplication2.AccountServices
{
    public interface IAccountDetails
    {
        Task<List<DetailDto>> GetDetails(string accountId);
    }
}