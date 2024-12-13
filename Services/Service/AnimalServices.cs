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

        public object? GetAnimalInfo(long? animalId)
        {
            var animal = context.Animal.FirstOrDefault(x => x.Id == animalId);

            if (animal == null)
                return null;
            
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

        public object? SearchAnimal(DateTime? startDateTime,
                                    DateTime? endDateTime,
                                    int? chipperId,
                                    long? chippingLocationId,
                                    string? lifeStatus,
                                    string? gender,
                                    int from = 0,
                                    int size = 10)
        {
            var animals = context.Animal.ToList();
            var visitedLocations = context.Animal_Location.ToList();
            var animalTypes = context.Animal_AnimalType.ToList();

            if (startDateTime != null)
            {
                //DateTime time = (DateTime)startDateTime;
                animals = animals.Intersect(SearchByStartDateTime(startDateTime)).ToList();
            }

            if (endDateTime != null)
            {
                animals = animals.Intersect(SearchByEndDateTime(endDateTime)).ToList();
            }

            if (chipperId != null)
            {
                animals = animals.Intersect(SearchByChipperId(chipperId)).ToList();
            }

            if (chippingLocationId != null)
            {
                animals = animals.Intersect(SearchByChippingLocationId(chippingLocationId)).ToList();
            }

            if (lifeStatus != null)
            {
                animals = animals.Intersect(SearchByLifeStatus(lifeStatus)).ToList();
            }

            if (gender != null)
            {
                animals = animals.Intersect(SearchByGender(gender)).ToList();
            }

            animals = animals.OrderBy(x => x.Id).ToList();

            List<object> result = new();
            for (int i = 0; i < animals.Count; i++) {
                result.Add( new
                {
                    animals[i].Id,
                    animals[i].Weight,
                    animals[i].Length,
                    animals[i].Height,
                    animals[i].ChipperId,
                    animals[i].ChippingLocationId,
                    animals[i].ChippingDateTime,
                    animals[i].DeathDateTime,
                    animals[i].LifeStatus,
                    animals[i].Gender,
                    VisitedLocations = visitedLocations.Where(x => x.AnimalId == animals[i].Id).Select(x => x.LocationId).ToList(),
                    AnimalTypes = animalTypes.Where(x => x.AnimalId == animals[i].Id).Select(x => x.AnimalTypeId).ToList(),
                });
            }
            

            try
            {
                result.RemoveRange(0, from);
                result.RemoveRange(size, result.Count - size);
            }
            catch { }

            return result;
        }

        private List<Animal> SearchByStartDateTime(DateTime? startDateTime)
        {
            
            //startDateTime = DateTime.SpecifyKind(startDateTime, DateTimeKind.Local);
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
                return context.Animal.Where(x => x.LifeStatus == AnimalLifeStatus.ALIVE.ToString()).ToList();
            if (lifeStatus.ToLower() == "dead")
                return context.Animal.Where(x => x.LifeStatus == AnimalLifeStatus.DEAD.ToString()).ToList();
            return new List<Animal>();
        }

        private List<Animal> SearchByGender(string gender)
        {
            if (gender == "male")
                return context.Animal.Where(x => x.Gender == AnimalGender.MALE.ToString()).ToList();
            if (gender == "female")
                return context.Animal.Where(x => x.Gender == AnimalGender.FEMALE.ToString()).ToList();
            if (gender == "other")
                return context.Animal.Where(x => x.Gender == AnimalGender.OTHER.ToString()).ToList();
            return new List<Animal>();
        }
    }
}
