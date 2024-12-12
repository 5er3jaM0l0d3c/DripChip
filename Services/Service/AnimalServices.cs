using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class AnimalServices : IAnimal
    {
        private DripChipContext context {  get; set; }
        public AnimalServices(DripChipContext context)
        {
            this.context = context;
        }

        public object GetAnimalInfo(long animalId)
        {
            var animal = context.Animal.FirstOrDefault(x => x.Id == animalId);

            if (animal == null)
                throw new Exception("400");
            
            var result = new
            {

                animal.Id,
                animal.Weight,
                animal.Length,
                animal.Height,
                animal.ChipperId,
                animal.ChippingLocationId,
                animal.ChippingDateTime,
                animal.DeathDateTime,
                animal.LifeStatus,
                animal.Gender,
                VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animalId).Select(x => x.LocationId).ToList(),
                AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animalId).Select(x => x.AnimalTypeId).ToList(),
            };

            return result;
        }
    }
}
