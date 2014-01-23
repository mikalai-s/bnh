using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackupTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnh.Tests
{
    [TestClass]
    public class BackupTool
    {
        [TestMethod]
        public void ConfigParser_GetParameterNames()
        {
            var field = "Hello World {ABC} Hello";
            var parameters = Config.GetParameterNames(field).ToArray();

            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("ABC", parameters[0]);


            field = "Hello {World}{ABC} Hello";
            parameters = Config.GetParameterNames(field).ToArray();

            Assert.AreEqual(2, parameters.Length);
            Assert.AreEqual("World", parameters[0]);
            Assert.AreEqual("ABC", parameters[1]);


            field = "{One}";
            parameters = Config.GetParameterNames(field).ToArray();

            Assert.AreEqual(1, parameters.Length);
            Assert.AreEqual("One", parameters[0]);
        }
    }
}
