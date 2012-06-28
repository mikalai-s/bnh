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
            using (var cms = new CmsEntities(cs))
            {
                var s = cms.Scenes.Collection.Find(Query.EQ("Walls.Bricks.BrickContentId", ObjectId.Parse("4fe94834afce1524345bd6eb"))).First();

                /*

                //cms.BrickContents.Insert(new HtmlContent() { Html = "abc" });
                var e = cms.BrickContents.Collection.FindOneById(ObjectId.Parse("4fe94433afce151fe43622f5"));

                cms.BrickContents.Collection.Remove(Query.EQ("_id", "4fe93c7eafce150ff868d7c9"));
               // var error = MongoDatabase.Create(cs).GetLastError();
                var bricks = cms.BrickContents.ToList();
                Assert.AreEqual(bricks.Count, 0);

                */
            }
        }
    }
}
