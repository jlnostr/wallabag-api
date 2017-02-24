using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;
using wallabag.Api.Responses;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class ModifyTests : TestBaseClass
    {
        private WallabagItem _sampleItem;

        [TestMethod]
        public void ModifyingFailsWhenItemIdIsMissing()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await Client.ArchiveAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await Client.UnarchiveAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await Client.FavoriteAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await Client.UnfavoriteAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await Client.DeleteAsync(0); });
        }

        [TestMethod]
        public async Task ItemIsArchivedAndUnarchived()
        {
            var itemId = (await Client.GetItemsAsync()).First().Id;

            Assert.IsTrue(await Client.ArchiveAsync(itemId));
            Assert.IsTrue((await Client.GetItemAsync(itemId)).IsRead);
            Assert.IsTrue(await Client.UnarchiveAsync(itemId));
            Assert.IsTrue((await Client.GetItemAsync(itemId)).IsRead == false);
        }

        [TestMethod]
        public async Task ItemIsStarredAndUnstarred()
        {
            var itemId = (await Client.GetItemsAsync()).First().Id;

            Assert.IsTrue(await Client.FavoriteAsync(itemId));
            Assert.IsTrue((await Client.GetItemAsync(itemId)).IsStarred);
            Assert.IsTrue(await Client.UnfavoriteAsync(itemId));
            Assert.IsTrue((await Client.GetItemAsync(itemId)).IsStarred == false);
        }

        [TestMethod]
        public async Task ItemIsDeleted()
        {
            var item = (await Client.GetItemsAsync()).First();

            Assert.IsTrue(await Client.DeleteAsync(item.Id));

            var items = (await Client.GetItemsAsync()).ToList();
            CollectionAssert.DoesNotContain(items, item);
        }

        [TestMethod]
        public void ItemsOfAManuallyCreatedItemCollectionResponseCanBeModified()
        {
            var resp = new ItemCollectionResponse();
            Assert.IsNull(resp.Items);

            resp.Items = new List<WallabagItem>();
            Assert.IsNotNull(resp.Items);
        }

        public override async Task InitializeAsync()
        {
            _sampleItem = await Client.AddAsync(
               uri: new Uri("https://www.wallabag.org/blog/2016/12/07/wallabagit"),
              tags: new string[] { "wallabag", "test", "subscription" });

            ItemsToDelete.Add(_sampleItem);
        }
    }
}
