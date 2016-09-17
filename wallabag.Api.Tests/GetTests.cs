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
        [TestCategory("Get")]
        public async Task AreItemsRetrieved()
        {
            List<WallabagItem> items = (await client.GetItemsAsync()).ToList();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        [TestCategory("Get")]
        public async Task ItemRetrievedById()
        {
            List<WallabagItem> items = (await client.GetItemsAsync()).ToList();

            var firstItem = items.First();
            var singleItem = await client.GetItemAsync(firstItem.Id);

            Assert.AreEqual(firstItem, singleItem);
        }

        [TestMethod]
        [TestCategory("Get")]
        public async Task ItemsRetrievedWithOneFilter()
        {
            List<WallabagItem> items = (await client.GetItemsAsync(isRead: true)).ToList();

            foreach (var item in items)
                Assert.IsTrue(item.IsRead);

            Assert.IsTrue(items.Count > 0);
        }

        /*
        [TestMethod]
        [TestCategory("Get")]
        public async Task ItemsRetrievedWithSpecificTag()
        {
            List<WallabagItem> items = (await client.GetItemsAsync(Tags: new string[] { "politik" })).ToList();

            foreach (var item in items)
                StringAssert.Contains(item.Tags.ToCommaSeparatedString(), "politik");

            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        [TestCategory("Get")]
        public async Task ItemsRetrievedWithMultipleTags()
        {
            List<WallabagItem> items = (await client.GetItemsAsync(Tags: new string[] { "politik", "test" })).ToList();

            foreach (var item in items)
            {
                StringAssert.Contains(item.Tags.ToCommaSeparatedString(), "politik");
                StringAssert.Contains(item.Tags.ToCommaSeparatedString(), "test");
            }

            Assert.IsTrue(items.Count > 0);
        }
        */

        [TestMethod]
        [TestCategory("Get")]
        public async Task ItemsRetrievedWithMultipleFilters()
        {
            List<WallabagItem> items = (await client.GetItemsAsync(isRead: true,
                isStarred: false,
                pageNumber: 1,
                itemsPerPage: 1)).ToList();

            var firstItem = items.First();

            Assert.IsTrue(items.Count == 1);
            Assert.IsTrue(firstItem.IsStarred == false);
            Assert.IsTrue(firstItem.IsRead == true);
        }

        [TestMethod, TestCategory("Get")]
        public async Task ItemsRetrievedWithSinceParameter()
        {
            var referenceDateTime = new DateTime(2016, 09, 01);

            List<WallabagItem> items = (await client.GetItemsAsync(since: referenceDateTime)).ToList();

            var firstItem = items.First();

            Assert.IsTrue(firstItem.CreationDate > referenceDateTime);
            foreach (var item in items)
                Assert.IsTrue(item.CreationDate > referenceDateTime);
        }

        [TestMethod]
        [TestCategory("Get")]
        public async Task AllPreviewImageUrisAreAbsolute()
        {
            List<WallabagItem> items = (await client.GetItemsAsync(itemsPerPage: 999999)).ToList();

            CollectionAssert.AllItemsAreUnique(items);
            Assert.IsTrue(items.Count > 0);

            foreach (var item in items)
                Assert.IsFalse(item.PreviewImageUri?.IsAbsoluteUri == false);
        }

        [TestMethod]
        [TestCategory("Get")]
        public async Task ExecutionOfInvalidRequestReturnsNull()
        {
            var accessToken = client.AccessToken;
            var refreshToken = client.RefreshToken;

            client.AccessToken = "veryrandomkey";
            client.RefreshToken = "anotherrandombullshit";

            var singleItem = await client.GetItemAsync(1337);
            Assert.IsNull(singleItem);

            var multipleItems = await client.GetItemsAsync(itemsPerPage: 1337);
            Assert.IsNull(multipleItems);

            var multipleItemsWithMetadata = await client.GetItemsWithEnhancedMetadataAsync(itemsPerPage: 1337);
            Assert.IsNull(multipleItemsWithMetadata);

            var unarchivedItem = await client.UnarchiveAsync(1337);
            Assert.IsFalse(unarchivedItem);

            var starredItem = await client.FavoriteAsync(1337);
            Assert.IsFalse(starredItem);

            client.AccessToken = accessToken;
            client.RefreshToken = refreshToken;
        }
    }
}
