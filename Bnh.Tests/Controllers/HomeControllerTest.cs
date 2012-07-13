using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bnh.Web;
using Bnh.Web.Controllers;
using Ms.Cms.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;
using Bnh.Entities;
using Bnh.Controllers;
using System.Web.Script.Serialization;

namespace Bnh.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestInitialize]
        public void Setup()
        {
            BsonClassMap.LookupClassMap(typeof(HtmlContent));
        }

        [TestMethod]
        public void Db()
        {
            var cs = "mongodb://127.0.0.1/bnh?safe=true";
            using (var db = new BleEntities(cs))
            {
                var city = db.Cities.First(c => c.Name == "Calgary");
                var zones = city.Zones.ToList();
                var communities = city
                    .GetCommunities(db)
                    .GroupBy(
                        c => c.Zone,
                        c => c)
                    .OrderBy(g => zones.IndexOf(g.Key))
                    .ToDictionary(
                        g => g.Key,
                        g => g.OrderBy(c => c.Name));
                    
                var s = new JavaScriptSerializer().Serialize(communities);
            }
        }
    }
}
