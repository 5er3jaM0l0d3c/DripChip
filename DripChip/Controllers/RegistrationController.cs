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
                return StatusCode(400);

            if (User.Identity != null && User.Identity.IsAuthenticated)
                return StatusCode(403);

            Account? acc;

            try
            {
                acc = Account.Registration(account);
            }
            catch (Exception ex)
            {
                return StatusCode(Int32.Parse(ex.Message));
            }

            if (acc != null)
            {
                return new JsonResult(acc);
            }

            return StatusCode(400);
        }
    }
}
