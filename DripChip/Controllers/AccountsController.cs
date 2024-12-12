using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace DripChip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccount Account { get; set; }
        public AccountsController(IAccount account)
        {
            Account = account;
        }

        [Authorize]
        [HttpGet("/accounts/{accountId}")]
        public IActionResult GetAccount(int accountId)
        {
            if (accountId == null || accountId <= 0)
                return StatusCode(400, "accountId = null,\naccountId <=0");
            
            Account? account = Account.GetAccountInfo(accountId);
            if (account != null)
            {
                account.Password = null;
                return new JsonResult(account);
            }
            return StatusCode(404, "Аккаунт с таким accountId не найден");
        }

        [Authorize]
        [HttpGet("/accounts/search")]
        public IActionResult Search([FromQuery] string? firstName,
                                    [FromQuery] string? lastName,
                                    [FromQuery] string? email,
                                    [FromQuery] int from = 0,
                                    [FromQuery] int size = 10)
        {
            if (from < 0)
                return StatusCode(400, "from < 0");
            List<Account> accounts = Account.Search(firstName, lastName, email, from, size);

            return new JsonResult(accounts);
        }
    }
}
