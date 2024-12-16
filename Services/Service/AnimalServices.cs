using Entities;
using Entities.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
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
            animal.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animalId).Select(x => x.LocationId).ToList();//BUG
            animal.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animalId).Select(x => x.AnimalTypeId).ToList();
            
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
                    animal.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animal.Id).Select(x => x.AnimalTypeId).ToList();
                    animal.AnimalTypes = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.LocationId).ToList();
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
                animal.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animal.Id).Select(x => x.AnimalTypeId).ToList();
                animal.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.LocationId).ToList();
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
            result.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.Id).ToList();
            result.AnimalTypes = context.Animal_AnimalType.Where(x => x.AnimalId == animal.Id).Select(x => x.AnimalTypeId).ToList();

            return result;

        }

        public Animal? UpdateAnimal(AnimalDTO animal)
        {
            var oldAnimal = context.Animal.FirstOrDefault(x => x.Id == animal.Id);

            if (oldAnimal == null
                || context.Account.AsNoTracking().FirstOrDefault(x => x.Id == animal.ChipperId) == null
                || context.Location.AsNoTracking().FirstOrDefault(x => x.Id == animal.ChippingLocationId) == null)
                throw new Exception("404");

            oldAnimal.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == animal.Id).Select(x => x.LocationId).ToList();

            if (oldAnimal.LifeStatus == "DEAD" && animal.LifeStatus == "ALIVE"
                || oldAnimal.VisitedLocations.FirstOrDefault() == animal.ChippingLocationId)
                throw new Exception("400");

            oldAnimal.Weight = animal.Weight;
            oldAnimal.Length = animal.Length;
            oldAnimal.Height = animal.Height;
            oldAnimal.Gender = animal.Gender.ToUpper();
            oldAnimal.LifeStatus = animal.LifeStatus.ToUpper();
            oldAnimal.ChipperId = animal.ChipperId;
            oldAnimal.ChippingLocationId = animal.ChippingLocationId;

            context.Entry(oldAnimal).State = EntityState.Modified;
            context.SaveChanges();

            oldAnimal.VisitedLocations = context.Animal_Location.Where(x => x.AnimalId == oldAnimal.Id).Select(x => x.Id).ToList();

            return context.Animal.FirstOrDefault(x => x.Id == animal.Id);
            
        }

        public void DeleteAnimal(long animalId)
        {
            var animal = context.Animal.AsNoTracking().FirstOrDefault(x => x.Id == animalId) ?? throw new Exception("404");
            animal.VisitedLocations = context.Animal_Location.AsNoTracking().Where(x => x.AnimalId == animalId).Select(x => x.Id).ToList();

            if (animal.VisitedLocations.Count > 1)
                throw new Exception("400");

            context.Animal.Remove(animal);
            context.SaveChanges();
        }

        public Animal? AddAnimalTypeToAnimal(long animalId, long typeId)
        {
            var animal = context.Animal.AsNoTracking().FirstOrDefault(x => x.Id == animalId);
            var animalType = context.AnimalType.AsNoTracking().FirstOrDefault(y => y.Id == typeId);

            if (animal == null || animalType == null)
                throw new Exception("404");           

            context.Animal_AnimalType.Add(new AnimalAnimalType { AnimalId = animalId, AnimalTypeId = typeId });
            context.SaveChanges();

            animal.AnimalTypes = context.Animal_AnimalType.AsNoTracking().Where(x => x.AnimalId == animalId).Select(x => x.AnimalTypeId).ToList();
            animal.VisitedLocations = context.Animal_Location.AsNoTracking().Where(x => x.AnimalId == animalId).Select(x => x.Id).ToList();

            return animal;
        }

        public Animal? UpdateAnimalTypeToAnimal(long animalId, NewOldAnimalTypeDTO types)
        {
            var animal = context.Animal.AsNoTracking().FirstOrDefault(x => x.Id == animalId);

            if (animal == null
                || context.AnimalType.FirstOrDefault(x => x.Id == types.OldTypeId) == null
                || context.AnimalType.FirstOrDefault(x => x.Id == types.NewTypeId) == null
                || context.Animal_AnimalType.FirstOrDefault(x => x.AnimalTypeId == types.OldTypeId && x.AnimalId == animalId) == null) throw new Exception("404");

            animal.VisitedLocations = context.Animal_Location.AsNoTracking().Where(x => x.AnimalId == animalId).Select(x => x.Id).ToList();
            var Types = context.Animal_AnimalType.AsNoTracking().Where(x => x.AnimalId == animalId).Select(x => x.AnimalTypeId).ToList();

            if (Types.Contains(types.NewTypeId)
                || Types.Contains(types.OldTypeId) && Types.Contains(types.NewTypeId))
                throw new Exception("409");

            var oldT = context.Animal_AnimalType.FirstOrDefault(x => x.AnimalId == animalId && x.AnimalTypeId == types.OldTypeId);
            var newT = new AnimalAnimalType { AnimalId = animalId, AnimalTypeId = types.NewTypeId };

            context.Animal_AnimalType.Remove(oldT);
            context.Animal_AnimalType.Add(newT);

            context.SaveChanges();

            animal.AnimalTypes = context.Animal_AnimalType.AsNoTracking().Where(x => x.AnimalId == animalId).Select(x => x.AnimalTypeId).ToList();

            return animal;
        }
    }
}
