using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Entities
{
    public static class CityExtensions
    {
        public static IEnumerable<Community> GetCommunities(this City city)
        {
            using (var db = new BleEntities())
            {
                return city.GetCommunities(db);
            }
        }

        public static IEnumerable<Community> GetCommunities(this City city, BleEntities db)
        {
            return db.Communities.Where(c => c.CityId == city.CityId);
        }
    }
}
