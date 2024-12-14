using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAnimalType
    {
        public AnimalType? GetAnimalTypeInfo(long? id);
        public AnimalType? AddAnimalType(AnimalType animalType);
        public AnimalType? UpdateAnimalType(long id, AnimalType animalType);
        public void DeleteAnimalType(long id);
    }
}
