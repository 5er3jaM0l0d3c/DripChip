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
        public Account? GetAccount(int accountId)
        {
            var login = User.FindFirst("Name")?.Value;
            var password = User.FindFirst("Password")?.Value;
            
            Account? account = Account.GetAccountInfo(accountId);
            if (account != null && account.Password == password && account.Email == login)
            {
                account.Password = null;
                return account;
            }
            return null; //исправить на обработку ошибок
        }

    }
}
