using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bnh.Entities
{
    public static class InitData
    {
        public static void Init()
        {
            using (var db = new BleEntities())
            {
                // just a simple check whether there is need to initilize data
                if (db.Cities.Where(s => s.Name == "Calgary").Any()) { return; }

                // insert calgary
                var calgary = new City
                {
                    Name = "Calgary",
                    UrlId = "Calgary",
                    Zones = new[] { "NW", "NE", "SE", "SW" }
                };
                db.Cities.Insert(calgary);

                // insert communitities
                db.Communities.Insert(new Community { Name = "Saddlebrook", UrlId = "Saddlebrook", CityId = calgary.Id, Zone = "NE" });
                db.Communities.Insert(new Community { Name = "Auburn Bay", UrlId = "AuburnBay", CityId = calgary.Id, Zone = "SE" });
            }
        }
    }
}
