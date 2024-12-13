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
        [HttpGet("/locatins/{pointId}")]
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
    }
}
