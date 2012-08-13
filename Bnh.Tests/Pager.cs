using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bnh.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bnh.Tests
{
    [TestClass]
    public class Pager
    {
        [TestMethod]
        public void Test()
        {
            var ii = new [] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0};

            var pager1 = new Pager<int>(0, 3, ii.Length, ii);
            Assert.AreEqual(pager1.NumberOfPages, 4);
            Assert.IsTrue(pager1.PageItems.SequenceEqual(new [] { 9, 8, 7 }));

            var pager2 = new Pager<int>(1, 3, ii.Length, ii);
            Assert.AreEqual(pager2.NumberOfPages, 4);
            Assert.IsTrue(pager2.PageItems.SequenceEqual(new [] { 6, 5, 4 }));

            var pager3 = new Pager<int>(1, 3, ii.Length, ii);
            Assert.AreEqual(pager3.NumberOfPages, 4);
            Assert.IsTrue(pager3.PageItems.SequenceEqual(new [] { 6, 5, 4 }));

            var pager4 = new Pager<int>(2, 3, ii.Length, ii);
            Assert.AreEqual(pager4.NumberOfPages, 4);
            Assert.IsTrue(pager4.PageItems.SequenceEqual(new [] { 3, 2, 1 }));

            var pager5 = new Pager<int>(3, 3, ii.Length, ii);
            Assert.AreEqual(pager5.NumberOfPages, 4);
            Assert.IsTrue(pager5.PageItems.SequenceEqual(new [] { 0 }));

            var pager6 = new Pager<int>(0, 1000, ii.Length, ii);
            Assert.AreEqual(pager6.NumberOfPages, 1);
            Assert.IsTrue(pager6.PageItems.SequenceEqual(ii));
        }
    }
}
