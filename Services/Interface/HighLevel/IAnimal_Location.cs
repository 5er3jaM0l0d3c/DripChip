using Entities;
using Entities.DTO;
using Services.Interface.LowLevel;
using Services.Interface.MidLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface.HighLevel
{
    public interface IAnimal_Location : ICreatable<AnimalLocation>, IUpdateable<AnimalLocation>, IDeleteable<AnimalLocation>, ISearchable<AnimalLocation>
    {
    }
}
