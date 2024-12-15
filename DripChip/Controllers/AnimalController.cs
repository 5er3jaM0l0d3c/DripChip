using Entities;
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
                Animal.AddAnimal(animal);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                    return BadRequest();
                return BadRequest(ex.Message);
            }
        }

        private bool HaveNullOrNegativeElement(long[] array)
        {
            foreach(var item in array)
            {
                if (item == null || item <= 0)
                    return true;
            }
            return false;
        }
    }
}
