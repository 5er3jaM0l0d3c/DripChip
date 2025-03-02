using Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interface.HighLevel;
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

        public Location? AddLocation(Location location)
        {
            if (context.Location
                .FirstOrDefault(x => x.Longitude == location.Longitude && x.Latitude == location.Latitude) != null)
                throw new Exception("409");


            context.Location.Add(location);
            context.SaveChanges();

            return context.Location.FirstOrDefault(x => x.Longitude == location.Longitude && x.Latitude == location.Latitude);

        }

        public Location? UpdateLocation(long id, Location location)
        {
            location.Id = id;

            if (context.Location.AsNoTracking()
                .FirstOrDefault(x => x.Longitude == location.Longitude && x.Latitude == location.Latitude) != null)
                throw new Exception("409");

            if (context.Location.AsNoTracking().FirstOrDefault(x => x.Id == id) == null)
                throw new Exception("404");

            context.Location.Update(location);
            context.SaveChanges();

            return location;
        }

        public void DeleteLocation(long id)
        {
            var location = context.Location.FirstOrDefault(x => x.Id == id);
            if (location == null)
                throw new Exception("404");

            if (context.Animal_Location.FirstOrDefault(x => x.LocationId == id) != null)
                throw new Exception("400");

            context.Location.Remove(location);
            context.SaveChanges();
        }

        public Location? Get(long id)
        {
            throw new NotImplementedException();
        }

        public Location Update(long id, Location entity)
        {
            throw new NotImplementedException();
        }

        public Location Add(Location entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}
