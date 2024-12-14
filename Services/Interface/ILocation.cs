using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface ILocation
    {
        public Location? GetLocationInfo(long? id);
        public Location? AddLocation(Location location);
        public Location? UpdateLocation(long id, Location location);
    }
}
