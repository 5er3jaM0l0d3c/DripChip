using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            if (typeId == null || typeId <= 0)
                return StatusCode(400, "typeId = null,\ntypeId <= 0");

            var result = AnimalType.GetAnimalTypeInfo(typeId);

            if (result == null)
                return StatusCode(404, "Тип животно с таким typeId = " + typeId + " не найден");
            return new JsonResult(result);
        }

        [Authorize]
        [HttpPost("/animals/types")]
        public IActionResult AddAnimalType([FromBody] AnimalType animalType)
        {
            if (animalType.Type.IsNullOrEmpty())
                return StatusCode(400, "type = null,\ntype = \"\" или состоит из пробелов");

            AnimalType? result = new();
            try
            {
                result = AnimalType.AddAnimalType(animalType);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "409")
                    return StatusCode(409, "Тип животного с таким type уже существует");
                return StatusCode(400, ex.Message);
            }
        }

        [Authorize]
        [HttpPut("/animals/types/{typeId}")]
        public IActionResult UpdateAnimalType(long typeId, [FromBody] AnimalType animalType)
        {
            if (typeId <= 0 || animalType.Type.IsNullOrEmpty())
                return StatusCode(400);

            AnimalType? result = new();

            try
            {
                result = AnimalType.UpdateAnimalType(typeId, animalType);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "409")
                    return StatusCode(409, "Тип животного с таким type уже существует");
                if (ex.Message == "404")
                    return StatusCode(404, "Тип животного с таким typeId не найден");
                return StatusCode(400, ex.Message);
            }
        }
    }
}
