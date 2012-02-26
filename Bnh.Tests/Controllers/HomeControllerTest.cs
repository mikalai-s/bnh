using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bnh;
using Bnh.Controllers;
using Bnh.Entities;

namespace Bnh.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            CmEntities e = new CmEntities("metadata=res://*/Cm.csdl|res://*/Cm.ssdl|res://*/Cm.msl;provider=System.Data.SqlClient;provider connection string=\"data source=localhost\\sqlserver;initial catalog=bnh;integrated security=True;multipleactiveresultsets=True;App=EntityFramework\"");
            var bricks = e.Bricks.ToList();

            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
