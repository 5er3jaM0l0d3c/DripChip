using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace DripChip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAccount Account { get; set; }
        public AuthController(IAccount account)
        {
            Account = account;
        }

        [HttpGet("/auth/{login}/{password}")]
        public IActionResult Auth(string login, string password)
        {
            var response = Account.Auth(login, password);
            if(response != null)
                return Content(response);
            return StatusCode(400);
        }
    }
}
