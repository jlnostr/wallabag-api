using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class EqualityTests : TestBaseClass
    {
        [TestMethod]
        public void ItemsWithSameIdAreEqual()
        {
            var item1 = new WallabagItem() { Id = 1337 };
            var item2 = new WallabagItem() { Id = 1337 };
            
            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void ItemsWithDifferentIdsAreNotEqual()
        {
            var item1 = new WallabagItem() { Id = 1337 };
            var item2 = new WallabagItem() { Id = 1338 };

            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreNotEqual(item1, item2);
        }

        [TestMethod]
        public void ItemsWithSameIdAndSameModificationDateAreEqual()
        {
            var item1 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now };
            var item2 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now };

            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreEqual(item1, item2);
        }

        [TestMethod]
        public void ItemsWithSameIdAndDifferentModificationDateAreNotEqual()
        {
            var item1 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now };
            var item2 = new WallabagItem() { Id = 1337, LastUpdated = DateTime.Now - TimeSpan.FromMinutes(1) };

            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreNotEqual(item1, item2);
            Assert.IsFalse(item1.CompareTo(item2) == 0);
        }

        [TestMethod]
        public void ItemsWithDifferentIdAndSameTitleAreNotEqual()
        {
            var item1 = new WallabagItem() { Id = 1337, Title = "test" };
            var item2 = new WallabagItem() { Id = 1338, Title = "test" };

            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreNotEqual(item1, item2);
        }

        [TestMethod]
        public void TagsWithDifferentIdsAndSameLabelAreEqual()
        {
            var tag1 = new WallabagTag() { Id = 1, Label = "test" };
            var tag2 = new WallabagTag() { Id = 2, Label = "test" };

            Assert.IsNotNull(tag1);
            Assert.IsNotNull(tag2);
            Assert.AreEqual(tag1, tag2);
        }

        [TestMethod]
        public void TagsWithSameIdsAndSameLabelAreEqual()
        {
            var tag1 = new WallabagTag() { Id = 1, Label = "test" };
            var tag2 = new WallabagTag() { Id = 1, Label = "test" };

            Assert.IsNotNull(tag1);
            Assert.IsNotNull(tag2);
            Assert.AreEqual(tag1, tag2);
        }

        [TestMethod]
        public void TagsWithSameIdAndDifferentLabelAreNotEqual()
        {
            var tag1 = new WallabagTag() { Id = 1, Label = "lorem" };
            var tag2 = new WallabagTag() { Id = 1, Label = "ipsum" };

            Assert.IsNotNull(tag1);
            Assert.IsNotNull(tag2);
            Assert.AreNotEqual(tag1, tag2);
        }
    }
}
