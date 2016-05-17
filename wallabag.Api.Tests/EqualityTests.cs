using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Equality")]
        public void ItemsWithSameIdAreEqual()
        {
            var item1 = new WallabagItem() { Id = 1337 };
            var item2 = new WallabagItem() { Id = 1337 };

            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void ItemsWithDifferentIdsAreNotEqual()
        {
            var item1 = new WallabagItem() { Id = 1337 };
            var item2 = new WallabagItem() { Id = 1338 };

            Assert.AreNotEqual(item1, item2);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void ItemsWithSameIdAndSameModificationDateAreEqual()
        {
            var item1 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now };
            var item2 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now };

            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void ItemsWithSameIdAndDifferentModificationDateAreNotEqual()
        {
            var item1 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now };
            var item2 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now - TimeSpan.FromMinutes(1) };

            Assert.AreNotEqual(item1, item2);
            Assert.IsFalse(item1.CompareTo(item2) == 0);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void ItemsWithDifferentIdAndSameTitleAreNotEqual()
        {
            var item1 = new WallabagItem() { Id = 1337, Title = "test" };
            var item2 = new WallabagItem() { Id = 1338, Title = "test" };

            Assert.AreNotEqual(item1, item2);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void TagsWithDifferentIdsAndSameLabelAreEqual()
        {
            var tag1 = new WallabagTag() { Id = 1, Label = "test" };
            var tag2 = new WallabagTag() { Id = 2, Label = "test" };

            Assert.AreEqual(tag1, tag2);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void TagsWithSameIdsAndSameLabelAreEqual()
        {
            var tag1 = new WallabagTag() { Id = 1, Label = "test" };
            var tag2 = new WallabagTag() { Id = 1, Label = "test" };

            Assert.AreEqual(tag1, tag2);
        }

        [TestMethod]
        [TestCategory("Equality")]
        public void TagsWithSameIdAndDifferentLabelAreNotEqual()
        {
            var tag1 = new WallabagTag() { Id = 1, Label = "lorem" };
            var tag2 = new WallabagTag() { Id = 1, Label = "ipsum" };

            Assert.AreNotEqual(tag1, tag2);
        }
    }
}
