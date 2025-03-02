using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Interface.HighLevel;

namespace DripChip.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAccount Account { get; set; }
        public AuthenticationController(IAccount account)
        {
            Account = account;
        }

        [HttpGet("/auth/{login}/{password}")]
        public IActionResult Auth(string login, string password)
        {
            var response = Account.Auth(login, password);
            if (response != null)
                return Content(response);
            return StatusCode(401, "Неверные авторизационные данные");
        }

        [HttpPost("/registration")]
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
                acc = Account.Add(account);
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
