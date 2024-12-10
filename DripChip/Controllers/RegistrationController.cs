using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public Account Registration(Account account)
        {
            var acc = Account.Registration(account);
            if (acc != null)
            {
                return acc;
            }
            return null;  //исправить на обработку ошибок
        }
    }
}
