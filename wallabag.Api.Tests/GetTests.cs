using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class GetTests : TestBaseClass
    {
        [TestMethod]
        public async Task ItemsAreRetrieved()
        {
            List<WallabagItem> items = (await Client.GetItemsAsync()).ToList();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public async Task ItemRetrievedById()
        {
            List<WallabagItem> items = (await Client.GetItemsAsync()).ToList();

            var firstItem = items.First();
            var singleItem = await Client.GetItemAsync(firstItem.Id);

            Assert.AreEqual(firstItem, singleItem);
        }

        [TestMethod]
        public async Task ItemsRetrievedWithOneFilter()
        {
            List<WallabagItem> items = (await Client.GetItemsAsync(isRead: true)).ToList();

            foreach (var item in items)
                Assert.IsTrue(item.IsRead);

            Assert.IsTrue(items.Count > 0);
        }


        [TestMethod]
        public async Task ItemsRetrievedWithSpecificTag()
        {
            if (await Client.VersionEqualsAsync("2.1"))
            {
                List<WallabagItem> items = (await Client.GetItemsAsync(tags: new string[] { "politik" })).ToList();

                foreach (var item in items)
                    StringAssert.Contains(item.Tags.ToCommaSeparatedString(), "politik");
            }
        }

        [TestMethod]
        public async Task ItemsRetrievedWithMultipleTags()
        {
            if (await Client.VersionEqualsAsync("2.1"))
            {
                var sampleTag = (await Client.GetTagsAsync()).First();

                List<WallabagItem> items = (await Client.GetItemsAsync(tags: new string[] { sampleTag.Label })).ToList();

                foreach (var item in items)
                    StringAssert.Contains(item.Tags.ToCommaSeparatedString(), sampleTag.Label);
            }
        }


        [TestMethod]
        public async Task ItemsRetrievedWithMultipleFilters()
        {
            List<WallabagItem> items = (await Client.GetItemsAsync(isRead: true,
                isStarred: false,
                pageNumber: 1,
                itemsPerPage: 1)).ToList();

            var firstItem = items.First();

            Assert.IsTrue(items.Count == 1);
            Assert.IsTrue(firstItem.IsStarred == false);
            Assert.IsTrue(firstItem.IsRead == true);
        }

        [TestMethod]
        public async Task ItemsRetrievedWithSinceParameter()
        {
            if (await Client.VersionEqualsAsync("2.1"))
            {
                var referenceDateTime = new DateTime(2016, 09, 01);

                List<WallabagItem> items = (await Client.GetItemsAsync(since: referenceDateTime)).ToList();

                var firstItem = items.First();

                Assert.IsTrue(firstItem.LastUpdated > referenceDateTime);
                foreach (var item in items)
                    Assert.IsTrue(item.LastUpdated > referenceDateTime);
            }
        }

        [TestMethod]
        public async Task AllPreviewImageUrisAreAbsolute()
        {
            List<WallabagItem> items = (await Client.GetItemsAsync(itemsPerPage: 1000)).ToList();

            CollectionAssert.AllItemsAreUnique(items);
            Assert.IsTrue(items.Count > 0);

            foreach (var item in items)
                Assert.IsFalse(item.PreviewImageUri?.IsAbsoluteUri == false);
        }

        [TestMethod]
        public async Task ExecutionOfInvalidRequestReturnsNull()
        {
            var accessToken = Client.AccessToken;
            var refreshToken = Client.RefreshToken;

            Client.AccessToken = "veryrandomkey";
            Client.RefreshToken = "anotherrandombullshit";

            var singleItem = await Client.GetItemAsync(1337);
            Assert.IsNull(singleItem);

            var multipleItems = await Client.GetItemsAsync(itemsPerPage: 1337);
            Assert.IsNull(multipleItems);

            var multipleItemsWithMetadata = await Client.GetItemsWithEnhancedMetadataAsync(itemsPerPage: 1337);
            Assert.IsNull(multipleItemsWithMetadata);

            var unarchivedItem = await Client.UnarchiveAsync(1337);
            Assert.IsFalse(unarchivedItem);

            var starredItem = await Client.FavoriteAsync(1337);
            Assert.IsFalse(starredItem);

            Client.AccessToken = accessToken;
            Client.RefreshToken = refreshToken;
        }
    }
}
