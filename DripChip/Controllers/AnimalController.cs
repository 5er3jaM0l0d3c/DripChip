﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interface;
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

            return Ok(animal);
        }
    }
}
