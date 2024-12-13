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
    }
}
