using Entities;
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
    }
}
