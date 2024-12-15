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

        public Animal? GetAnimalInfo(long? animalId)
        {
            var animal = context.Animal.FirstOrDefault(x => x.Id == animalId);

            if (animal == null)
                return null;
                                                                                                           //x.Id
            animal.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animalId).Select(x => x.LocationId).ToArray();//BUG
            animal.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animalId).Select(x => x.AnimalTypeId).ToArray();
            
            return animal;
        }

        public  List<Animal>? SearchAnimal(DateTime? startDateTime,
                                    DateTime? endDateTime,
                                    int? chipperId,
                                    long? chippingLocationId,
                                    string? lifeStatus,
                                    string? gender,
                                    int from = 0,
                                    int size = 10)
        {

            List<Animal> animals = new();

            if (startDateTime == null
                && endDateTime == null
                && chipperId == null
                && chippingLocationId == null
                && lifeStatus == null
                && gender == null)
            {
                animals = context.Animal.Skip(from).Take(size).ToList();

                foreach (var animal in animals)
                {
                    animal.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animal.Id).Select(x => x.AnimalTypeId).ToArray();
                    animal.AnimalTypes = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.LocationId).ToArray();
                }

                return animals;
            }

            (animals, var filterNum) = GetFirstFilter(startDateTime, endDateTime, chipperId, chippingLocationId, lifeStatus, gender);

            if (filterNum != 0 && startDateTime != null)
            {
                animals = animals.Intersect(SearchByStartDateTime(startDateTime)).ToList();
            }

            if (filterNum != 1 && endDateTime != null)
            {
                animals = animals.Intersect(SearchByEndDateTime(endDateTime)).ToList();
            }

            if (filterNum != 2 && chipperId != null)
            {
                animals = animals.Intersect(SearchByChipperId(chipperId)).ToList();
            }

            if (filterNum != 3 && chippingLocationId != null)
            {
                animals = animals.Intersect(SearchByChippingLocationId(chippingLocationId)).ToList();
            }

            if (filterNum != 4 && lifeStatus != null)
            {
                animals = animals.Intersect(SearchByLifeStatus(lifeStatus)).ToList();
            }

            if (filterNum != 5 && gender != null)
            {
                animals = animals.Intersect(SearchByGender(gender)).ToList();
            }

            animals = animals.OrderBy(x => x.Id).ToList();

            foreach (var animal in animals)
            {
                animal.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animal.Id).Select(x => x.AnimalTypeId).ToArray();
                animal.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.LocationId).ToArray();
            }
            

            try
            {
                animals.RemoveRange(0, from);
                animals.RemoveRange(size, animals.Count - size);
            }
            catch { }

            return animals;
        }

        private (List<Animal>, int) GetFirstFilter(DateTime? startDateTime,
                                    DateTime? endDateTime,
                                    int? chipperId,
                                    long? chippingLocationId,
                                    string? lifeStatus,
                                    string? gender)
        {
            if (startDateTime != null)
            {
                return (SearchByStartDateTime(startDateTime).ToList(), 0);
            }

            if (endDateTime != null)
            {
                return (SearchByEndDateTime(endDateTime).ToList(), 1);
            }

            if (chipperId != null)
            {
                return (SearchByChipperId(chipperId).ToList(), 2);
            }

            if (chippingLocationId != null)
            {
                return (SearchByChippingLocationId(chippingLocationId).ToList(), 3);
            }

            if (lifeStatus != null)
            {
                return (SearchByLifeStatus(lifeStatus).ToList(), 4);
            }

            if (gender != null)
            {
                return (SearchByGender(gender).ToList(), 5);
            }
            throw new Exception();
        }

        private List<Animal> SearchByStartDateTime(DateTime? startDateTime)
        {
            return context.Animal.Where(x => x.ChippingDateTime >= startDateTime).ToList();
        }

        private List<Animal> SearchByEndDateTime(DateTime? endDateTime)
        {
            return context.Animal.Where(x => x.ChippingDateTime <= endDateTime).ToList();
        }

        private List<Animal> SearchByChipperId(int? chipperId)
        {
            return context.Animal.Where(x => x.ChipperId == chipperId).ToList();
        }

        private List<Animal> SearchByChippingLocationId(long? chippingLocationId)
        {
            return context.Animal.Where(x => x.ChippingLocationId == chippingLocationId).ToList();
        }

        private List<Animal> SearchByLifeStatus(string lifeStatus)
        {
            if(lifeStatus.ToLower() == "alive")
                return context.Animal.Where(x => x.LifeStatus.Equals("alive", StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (lifeStatus.ToLower() == "dead")
                return context.Animal.Where(x => x.LifeStatus.Equals("dead", StringComparison.CurrentCultureIgnoreCase)).ToList();
            return new List<Animal>();
        }

        private List<Animal> SearchByGender(string gender)
        {
            if (gender == "male")
                return context.Animal.Where(x => x.Gender.Equals("male", StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (gender == "female")                      
                return context.Animal.Where(x => x.Gender.Equals("female", StringComparison.CurrentCultureIgnoreCase)).ToList();
            if (gender == "other")                       
                return context.Animal.Where(x => x.Gender.Equals("other", StringComparison.CurrentCultureIgnoreCase)).ToList();
            return new List<Animal>();
        }

        public Animal AddAnimal(Animal animal)
        {
            foreach(var typeId in animal.AnimalTypes)
                if(context.AnimalType.AsNoTracking().FirstOrDefault(x => x.Id == typeId) == null)
                    throw new Exception("404");
            
            if(context.Account.AsNoTracking().FirstOrDefault(x => x.Id == animal.ChipperId) == null)
                throw new Exception("404");
            
            if(context.Location.AsNoTracking().FirstOrDefault(x => x.Id == animal.ChippingLocationId) == null)
                throw new Exception("404");

            animal.LifeStatus = "ALIVE";
            animal.Gender = animal.Gender.ToUpper();
            context.Animal.Add(animal);

            foreach (var typeId in animal.AnimalTypes)
                context.Animal_AnimalType.Add(new AnimalAnimalType 
                {
                    AnimalId = animal.Id,
                    AnimalTypeId = typeId
                });

            context.SaveChanges();

            var result = context.Animal.FirstOrDefault(x => x.Id == animal.Id);
            result.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.Id).ToArray();
            result.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animal.Id).Select(x => x.AnimalTypeId).ToArray();

            return result;

        }
    }
}
