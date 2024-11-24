using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.AccountServices;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateBalanceController : ControllerBase
    {
        private readonly ICalculateBalance CalculateBalance;
        private readonly IAccountDetails accountDetails;

        public CalculateBalanceController(ICalculateBalance calculateBalance, IAccountDetails accountDetails)
        {
            CalculateBalance = calculateBalance;
            this.accountDetails = accountDetails;
        }

        [HttpGet("GetCalculateBalance")]
        public async Task<IActionResult> GetCalculateBalance()
        {
            dynamic? Data;
            try
            {
                return Ok(Data = await CalculateBalance.CalTotalBalance());
            }
            catch (Exception e) { return BadRequest(e.InnerException?.Message ?? e.Message); }
        }
        [HttpGet("AccountDetails")]
        public async Task<IActionResult> AccountDetails(string accountId)
        {
            dynamic? Data;
            try
            {
                return Ok(Data = await accountDetails.GetDetails(accountId));
            }
            catch (Exception e) { return BadRequest(e.InnerException?.Message ?? e.Message); }
        }
    }
}
