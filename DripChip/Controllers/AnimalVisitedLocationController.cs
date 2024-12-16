using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace DripChipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalVisitedLocationController : ControllerBase
    {
        private IAnimal_Location Animal_Location { get; set; }
        public AnimalVisitedLocationController(IAnimal_Location Animal_Location)
        {
            this.Animal_Location = Animal_Location;
        }

        [Authorize]
        [HttpGet("/animals/{animalId}/locations")]
        public IActionResult GetAnimalLocationsInfo(long animalId, DateTime? startDateTime, DateTime? endDateTime, int from = 0, int size = 10)
        {
            if (animalId <= 0
                || from < 0
                || size <= 0)
                return StatusCode(400);

            List<AnimalLocation> result = new();
            try
            {
                result = Animal_Location.GetAnimalLocationInfo(animalId, startDateTime, endDateTime, from, size);
            }
            catch (Exception ex)
            {
                if (ex.Message == "404")
                    return StatusCode(404, "Животное с animalId = " + animalId + " не найдено");
            }

            return new JsonResult(result);
        }

        [Authorize]
        [HttpPost("/animals/{animalId}/locations/{pointId}")]
        public IActionResult AddAnimalLocation(long animalId, long pointId)
        {
            if (animalId <= 0 || pointId <= 0) return BadRequest();

            AnimalLocation? result = new();

            try
            {
                result = Animal_Location.AddAnimalLocation(animalId, pointId);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if(ex.Message == "400")
                    return BadRequest();
                if(ex.Message == "404")
                    return StatusCode(404);
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpPut("/animals/{animalId}/locations")]
        public IActionResult UpdateAnimalLocation(long animalId, [FromBody]AnimalLocationDTO animalLocation)
        {
            if(animalId <= 0 || animalLocation.LocationPointId <= 0 || animalLocation.VisitedLocationPointId <= 0) return BadRequest();

            AnimalLocation result = new();

            try
            {
                result = Animal_Location.UpdateAnimalLocation(animalId, animalLocation);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if(ex.Message == "404")
                    return StatusCode(404);
                if(ex.Message == "400")
                    return BadRequest();
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpDelete("/animals/{animalId}/locations/{visitedPointId}")]
        public IActionResult DeleteAnimalLocation(long animalId, long locationId)
        {
            if (animalId <= 0 || locationId <= 0) return BadRequest();

            try
            {
                Animal_Location.DeleteAnimalLocation(animalId, locationId);
                return Ok();
            }
            catch (Exception ex)
            {
                if(ex.Message == "404")
                    return NotFound();
                return BadRequest(ex);
            }
        }
    }
}
