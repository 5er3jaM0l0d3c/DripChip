using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;

namespace DripChip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccount Account { get; set; }
        public AccountController(IAccount account)
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
            return StatusCode(404, "Аккаунт с accountId = " + accountId + " не найден");
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

        [Authorize]
        [HttpPut("/accounts/{accountId}")]
        public IActionResult UpdateAccount(int accountId, [FromBody]Account account)
        {
            var userId = User.FindFirst("Id");

            if (userId != null && userId.Value != accountId.ToString())
                return StatusCode(403, "Обновление не своего аккаунта");

            Account result = new();

            if (accountId <= 0
                || account.FirstName.IsNullOrEmpty()
                || account.LastName.IsNullOrEmpty()
                || account.Email.IsNullOrEmpty())
                return StatusCode(400);

            try
            {
                result = Account.UpdateAccount(accountId, account);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "403")
                    return StatusCode(403, "Аккаунт не найден");
                if (ex.Message == "409")
                    return StatusCode(403, "Аккаунт с таким email уже существует");
            }

            return StatusCode(400, "Неизвестная ошибка");
        }

        [Authorize]
        [HttpDelete("/accounts/{accountId}")]
        public IActionResult DeleteAccount(int accountId)
        {
            if (accountId <= 0)
                return StatusCode(400, "accountId = null,\naccountId <= 0");

            var userId = User.FindFirst("Id");

            if (userId != null && userId.Value != accountId.ToString())
                return StatusCode(403, "Обновление не своего аккаунта");

            Account.DeleteAccount(accountId);
            return Ok();
        }
    }
}
