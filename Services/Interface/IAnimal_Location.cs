using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IAnimal_Location
    {
        public List<AnimalLocation> GetAnimalLocationInfo(long animalId, DateTime? startDateTime, DateTime? endDateTime, int from, int size);
    }
}
