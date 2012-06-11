using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bnh.Web;
using Bnh.Web.Controllers;
using Ms.Cms.Models;

namespace Bnh.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Db()
        {
            var db = new CmsEntities(@"data source=localhost\sqlserver;Initial Catalog=bnh;Integrated Security=SSPI;");
            var walls = db.Walls.ToList();
            Assert.IsTrue(walls.Count == 2);
            Assert.IsTrue(walls[0].Bricks.Count == 1);
            Assert.IsTrue(walls[1].Bricks.Count == 1);
        }
    }
}
