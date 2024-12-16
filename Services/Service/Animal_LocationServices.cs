using Entities;
using Entities.DTO;
using Microsoft.EntityFrameworkCore;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class Animal_LocationServices : IAnimal_Location
    {
        private DripChipContext context {  get; set; }
        public Animal_LocationServices(DripChipContext context)
        {
            this.context = context; 
        }

        public List<AnimalLocation> GetAnimalLocationInfo(long animalId, DateTime? startDateTime, DateTime? endDateTime, int from, int size)
        {
            if (context.Animal.FirstOrDefault(x => x.Id == animalId) == null)
                throw new Exception("404");

            List<AnimalLocation> result = context.Animal_Location.Where(x => x.AnimalId == animalId).ToList();

            if (startDateTime != null)
            {
                result = result.Intersect(SearchByStartDateTime(startDateTime)).ToList();
            }

            if (endDateTime != null)
            {
                result = result.Intersect(SearchByEndDateTime(endDateTime)).ToList();
            }

            result = result.OrderBy(x => x.DateTimeOfVisitLocationPoint).ToList();

            try
            {
                result.RemoveRange(0, from);
                result.RemoveRange(size, result.Count - size);
            }
            catch { }

            return result;
        }

        private List<AnimalLocation> SearchByStartDateTime(DateTime? startDateTime)
        {
            return context.Animal_Location.Where(x => x.DateTimeOfVisitLocationPoint >= startDateTime).ToList();
        }

        private List<AnimalLocation> SearchByEndDateTime(DateTime? endDateTime)
        {
            return context.Animal_Location.Where(x => x.DateTimeOfVisitLocationPoint <= endDateTime).ToList();
        }

        public AnimalLocation AddAnimalLocation(long animalId, long pointId)
        {
            var animal = context.Animal.AsNoTracking().FirstOrDefault(x => x.Id == animalId);
            var location = context.Location.AsNoTracking().FirstOrDefault(x => x.Id == pointId);

            if (animal == null || location == null) throw new Exception("404");

            if (animal.LifeStatus == "DEAD" || animal.ChippingLocationId == pointId) throw new Exception("400");

            animal.ChippingLocationId = pointId;

            context.Animal.Update(animal);

            return context.Animal_Location.FirstOrDefault(x => x.AnimalId == animalId && x.LocationId == pointId);
        }

        public AnimalLocation UpdateAnimalLocation(long animalId, AnimalLocationDTO animalLocation)
        {
            var animal = context.Animal.AsNoTracking().FirstOrDefault(x => x.Id == animalId);
            var animallocation = context.Animal_Location.AsNoTracking().FirstOrDefault(x => x.Id == animalLocation.VisitedLocationPointId);
            var location = context.Location.AsNoTracking().FirstOrDefault(x => x.Id == animalLocation.LocationPointId);

            if (animal == null 
                || animallocation == null 
                || location == null 
                || animallocation.AnimalId != animalId) throw new Exception("404");

            var animalVisitedLocations = context.Animal_Location.AsNoTracking().Where(x => x.AnimalId == animalId).ToList();


            animallocation.LocationId = animalLocation.LocationPointId;

            context.Animal_Location.Update(animallocation);
            context.SaveChanges();

            return animallocation;
        }
    }
}
