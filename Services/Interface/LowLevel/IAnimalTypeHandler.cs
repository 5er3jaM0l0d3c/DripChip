using Entities.DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.LowLevel
{
    public interface IAnimalTypeHandler
    {
        public Animal? AddAnimalTypeToAnimal(long animalId, long typeId);
        public Animal? UpdateAnimalTypeToAnimal(long animalId, NewOldAnimalTypeDTO types);
        public Animal? DeleteAnimalTypeToAnimal(long animalId, long typeId);
    }
}
