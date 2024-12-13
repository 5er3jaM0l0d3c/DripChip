using Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
