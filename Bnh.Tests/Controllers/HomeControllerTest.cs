using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bnh.Web;
using Bnh.Web.Controllers;
using Bnh.Cms.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using MongoDB.Driver;
using Bnh.Controllers;
using System.Web.Script.Serialization;
using Bnh.Infrastructure.Repositories;

namespace Bnh.Tests.Controllers
{/*
    [TestClass]
    public class HomeControllerTest
    {
        [TestInitialize]
        public void Setup()
        {
            //BsonClassMap.LookupClassMap(typeof(HtmlContent));
        }

        [TestMethod]
        public void Db()
        {
            var cs = "mongodb://127.0.0.1/bnh?safe=true";
            var db = new ReviewRepository(cs);
            db.AddReviewComment("503af0f8afce15288815273f", new Core.Entities.Comment { Message = "Asdf as" });
            
        }
    }*/
}
