using Entities;
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

        [HttpGet("/accounts/{accountId}")]
        public Account? GetAccount(int accountId)
        {
            Account? account = Account.GetAccountInfo(accountId);
            if (account != null)
            {
                return account;
            }
            return null; //исправить на обработку ошибок
        }

    }
}
