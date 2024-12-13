using Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service
{
    public class LocationServices : ILocation
    {
        private DripChipContext context {  get; set; }
        public LocationServices(DripChipContext context)
        {
            this.context = context;
        }

        public Location? GetLocationInfo(long? id)
        {
            return context.Location.FirstOrDefault(x => x.Id == id);
        }
    }
}
