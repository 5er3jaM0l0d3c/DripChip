using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Interface;

namespace DripChip.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private IAccount Account { get; set; }
        public RegistrationController(IAccount account)
        {
            Account = account;
        }

        [HttpPost]
        public IActionResult Registration(Account account)
        {
            if (String.IsNullOrWhiteSpace(account.Password)
                || String.IsNullOrWhiteSpace(account.FirstName)
                || String.IsNullOrWhiteSpace(account.LastName)
                || String.IsNullOrWhiteSpace(account.Email))
                return StatusCode(400, "Поле = null, =\"\" или состоит из пробелов");

            if (User.Identity != null && User.Identity.IsAuthenticated)
                return StatusCode(403, "Запрос от авторизованного аккаунта");

            Account? acc = null;

            try
            {
                acc = Account.Registration(account);
            }
            catch (Exception ex)
            {
                if(ex.Message == "409")
                    return StatusCode(Int32.Parse(ex.Message), "Аккаунт с таким email уже существует");
            }

            if (acc != null)
            {
                return new JsonResult(acc);
            }

            return StatusCode(400);
        }
    }
}
