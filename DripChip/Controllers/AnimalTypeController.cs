using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace DripChipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalTypeController : ControllerBase
    {
        private IAnimalType AnimalType { get; set; }
        public AnimalTypeController(IAnimalType animalType)
        {
            AnimalType = animalType;
        }

        [Authorize]
        [HttpGet("/animals/types/{typeId}")]
        public IActionResult GetAnimalTypeInfo(int? typeId)
        {
            if(typeId == null || typeId <= 0)
                return StatusCode(400, "typeId = null,\ntypeId <= 0");

            var result = AnimalType.GetAnimalTypeInfo(typeId);

            if (result == null)
                return StatusCode(404, "Тип животно с таким typeId = " + typeId + " не найден");
            return new JsonResult(result);
        }
    }
}
