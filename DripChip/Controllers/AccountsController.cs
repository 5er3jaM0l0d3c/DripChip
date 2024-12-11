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

            var login = User.FindFirst("Name")?.Value;
            var password = User.FindFirst("Password")?.Value;
            
            Account? account = Account.GetAccountInfo(accountId);
            if (account != null)
            {
                if (account.Password != password || account.Email != login)
                    return StatusCode(401,"Неверные авторизационные данные");
                account.Password = null;
                return new JsonResult(account);
            }
            return StatusCode(404, "Аккаунт с таким accountId не найден");
        }

    }
}
