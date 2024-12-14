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
    public class AnimalTypeServices : IAnimalType
    {
        private DripChipContext context {  get; set; }
        public AnimalTypeServices(DripChipContext context)
        {
            this.context = context;
        }

        public AnimalType? GetAnimalTypeInfo(long? id)
        {
            return context.AnimalType.FirstOrDefault(x => x.Id == id);
        }

        public AnimalType? AddAnimalType(AnimalType animalType)
        {
            if (context.AnimalType.FirstOrDefault(x => x.Type == animalType.Type) != null)
                throw new Exception("409");

            context.AnimalType.Add(animalType);
            context.SaveChanges();

            return animalType;
        }

        public AnimalType? UpdateAnimalType(long id, AnimalType animalType)
        {
            if (context.AnimalType.AsNoTracking().FirstOrDefault(y => y.Id == id) == null)
                throw new Exception("404");

            if (context.AnimalType.FirstOrDefault(x => x.Type == animalType.Type) != null)
                throw new Exception("409");

            animalType.Id = id;

            context.AnimalType.Update(animalType);
            context.SaveChanges();

            return animalType;
        }

        public void DeleteAnimalType(long id)
        {
            var animalType = context.AnimalType.FirstOrDefault(x => x.Id == id) ?? throw new Exception("404");

            if (context.Animal_AnimalType.FirstOrDefault(x => x.AnimalType == animalType) != null)
                throw new Exception("400");

            context.AnimalType.Remove(animalType);
        }
    }
}
