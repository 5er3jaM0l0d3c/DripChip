using Entities;
using Entities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAnimal
    {
        public Animal? GetAnimalInfo(long? animalId);
        public List<Animal>? SearchAnimal(DateTime? startDateTime,
                                    DateTime? endDateTime,
                                    int? chipperId,
                                    long? chippingLocationId,
                                    string? lifeStatus,
                                    string? gender,
                                    int from = 0,
                                    int size = 10);
        public Animal AddAnimal(Animal animal);
        public Animal? UpdateAnimal(AnimalDTO animal);
    }
}
