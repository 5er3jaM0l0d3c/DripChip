using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
using System.Reflection;
using System.Text;

namespace DripChipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : ControllerBase
    {
        private IAnimal Animal { get; set; }
        public AnimalController(IAnimal animal)
        {
            Animal = animal;
        }

        [Authorize]
        [HttpGet("/animals/{animalId}")]
        public IActionResult GetAnimalInfo(long? animalId)
        {
            if (animalId == null || animalId <= 0)
                return StatusCode(400, "animalId = null\nanimalId <= 0");

            var animal = Animal.GetAnimalInfo(animalId);

            if (animal == null)
                return StatusCode(404, "Животное с animalId = " + animalId + " не найдено");

            return new JsonResult(animal);
        }

        [Authorize]
        [HttpGet("/animals/search")]
        public IActionResult SearchAnimals([FromQuery] DateTime? startDateTime,
                                           [FromQuery] DateTime? endDateTime,
                                           [FromQuery] int? chipperId,
                                           [FromQuery] long? chippingLocationId,
                                           [FromQuery] string? lifeStatus,
                                           [FromQuery] string? gender,
                                           [FromQuery] int from = 0,
                                           [FromQuery] int size = 10)
        {
            if (from < 0 || size <= 0
                || chipperId != null && chipperId <= 0
                || chippingLocationId != null && chippingLocationId <= 0
                || lifeStatus != null && lifeStatus.ToLower() != "alive" && lifeStatus.ToLower() != "dead"
                || gender != null && gender.ToLower() != "male" && gender.ToLower() != "female" && gender.ToLower() != "other")
                return BadRequest();

            var result = Animal.SearchAnimal(startDateTime, endDateTime, chipperId, chippingLocationId, lifeStatus, gender, from, size);

            return new JsonResult(result);
        }

        [Authorize]
        [HttpPost("/animals")]
        public IActionResult AddAnimal([FromBody] Animal animal)
        {
            if (animal.AnimalTypes == null
                || animal.AnimalTypes.Count() <= 0
                || HaveNullOrNegativeElement(animal.AnimalTypes)
                || animal.Weight == null || animal.Weight <= 0
                || animal.Length == null || animal.Length <= 0
                || animal.Height == null || animal.Height <= 0
                || animal.Gender == null || (animal.Gender.ToLower() != "male" && animal.Gender.ToLower() != "female" && animal.Gender.ToLower() != "other")
                || animal.ChipperId == null || animal.ChipperId <= 0
                || animal.ChippingLocationId == null || animal.ChippingLocationId <= 0)
                return BadRequest();

            Animal result = new();

            try
            {
                result = Animal.AddAnimal(animal);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                    return StatusCode(404, "Тип животного не найден ИЛИ аккаунт с chipperId не найден ИЛИ точка локации с chippingLocationId не найдена");
                return BadRequest(ex.Message);
            }
        }

        private bool HaveNullOrNegativeElement(List<long> array)
        {
            foreach (var item in array)
            {
                if (item == null || item <= 0)
                    return true;
            }
            return false;
        }

        [Authorize]
        [HttpPut("/animals/{animalId}")]
        public IActionResult UpdateAnimal(long animalId, [FromBody] AnimalDTO animal)
        {
            if (animal.Weight == null || animal.Weight <= 0
                || animal.Length == null || animal.Length <= 0
                || animal.Height == null || animal.Height <= 0
                || animal.Gender == null || (animal.Gender.ToLower() != "male" && animal.Gender.ToLower() != "female" && animal.Gender.ToLower() != "other")
                || animal.LifeStatus == null || (animal.LifeStatus.ToLower() != "alive" && animal.LifeStatus.ToLower() != "dead")
                || animal.ChipperId == null || animal.ChipperId <= 0
                || animal.ChippingLocationId == null || animal.ChippingLocationId <= 0)
                return BadRequest();

            Animal? result = new();

            try
            {
                animal.Id = animalId;
                result = Animal.UpdateAnimal(animal);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "400")
                    return BadRequest("Установка lifeStatus = \"ALIVE\" когда у животного lifeStatus = \"DEAD\" " +
                        "ИЛИ новая точка чипирования совпадает с первой посещенной точкой локации");
                if (ex.Message == "404")
                    return StatusCode(404, "Животное с animalId не найдено ИЛИ аккаунт с chipperId не найден ИЛИ точка локации с chippingLocationId не найдена");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("/animals/{animalId}")]
        public IActionResult DeleteAnimal(long animalId)
        {
            if (animalId <= 0) return BadRequest("animalId <= 0");

            try
            {
                Animal.DeleteAnimal(animalId);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message == "400")
                    return StatusCode(404, "Животное покинуло локацию чипирования, при этом есть другие посещенные точки");
                if (ex.Message == "404")
                    return StatusCode(404, "Животное с animalId не найдено");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("/animals/{animalId}/types/{typeId}")]
        public IActionResult AddAnimalTypeToAnimal(long animalId, long typeId)
        {
            if (animalId <= 0 || typeId <= 0)
                return BadRequest();

            Animal? result = new();

            try
            {
                result = Animal.AddAnimalTypeToAnimal(animalId, typeId);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                    return StatusCode(404, "Животное с animalId = " + animalId + " не найдено ИЛИ тип животного с typeId = " + typeId + " не надйен");
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPut("/animals/{animalId}/types")]
        public IActionResult UpdateAnimalTypeToAnimal(long animalId, [FromBody] NewOldAnimalTypeDTO types)
        {
            if (types.OldTypeId <= 0 ||
                types.NewTypeId <= 0 ||
                animalId <= 0) return BadRequest();

            Animal? result = new();

            try
            {
                result = Animal.UpdateAnimalTypeToAnimal(animalId, types);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                    return StatusCode(404);
                if (ex.Message == "409")
                    return StatusCode(409);
                return BadRequest(ex);
            }
        }
    }
}
