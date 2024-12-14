using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;

namespace DripChipAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private ILocation Location { get; set; }
        public LocationController(ILocation location)
        {
            Location = location;
        }

        [Authorize]
        [HttpGet("/locations/{pointId}")]
        public IActionResult GetLocationInfo(long? pointId)
        {
            if (pointId == null || pointId <= 0)
            {
                return StatusCode(400, "pointId = null,\npointId <= 0");
            }

            var result = Location.GetLocationInfo(pointId);

            if (result == null)
                return StatusCode(404, "Точка локации с таким pointId = " + pointId + " не найдена");
            return new JsonResult(result);
        }

        [Authorize]
        [HttpPost("/locations")]
        public IActionResult AddLocation([FromBody] Location location)
        {
            if (location.Latitude == null || location.Latitude < -90 || location.Latitude > 90
                || location.Longitude == null || location.Longitude < -180 || location.Longitude > 180)
                return StatusCode(400);
            Location result = new();
            try
            {
                result = Location.AddLocation(location);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                if(ex.Message == "409")
                    return StatusCode(409, "Точка локации с такими latitude = " + location.Latitude +
                        " и longitude = " + location.Longitude + " уже существует");
                return StatusCode(400, "Неизвестная ошибка");
            }
        }
    }
}
