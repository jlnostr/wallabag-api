using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Linq;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Modify")]
        public void ModifyingFailsWhenItemIdIsMissing()
        {
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await client.ArchiveAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await client.UnarchiveAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await client.FavoriteAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await client.UnfavoriteAsync(0); });
            AssertExtensions.ThrowsExceptionAsync<ArgumentNullException>(async () => { await client.DeleteAsync(0); });
        }

        [TestMethod]
        [TestCategory("Modify")]
        public async Task ItemIsArchivedAndUnarchived()
        {
            var itemId = (await client.GetItemsAsync()).First().Id;

            Assert.IsTrue(await client.ArchiveAsync(itemId));
            Assert.IsTrue((await client.GetItemAsync(itemId)).IsRead);
            Assert.IsTrue(await client.UnarchiveAsync(itemId));
            Assert.IsTrue((await client.GetItemAsync(itemId)).IsRead == false);
        }

        [TestMethod]
        [TestCategory("Modify")]
        public async Task ItemIsStarredAndUnstarred()
        {
            var itemId = (await client.GetItemsAsync()).First().Id;

            Assert.IsTrue(await client.FavoriteAsync(itemId));
            Assert.IsTrue((await client.GetItemAsync(itemId)).IsStarred);
            Assert.IsTrue(await client.UnfavoriteAsync(itemId));
            Assert.IsTrue((await client.GetItemAsync(itemId)).IsStarred == false);
        }

        [TestMethod]
        [TestCategory("Modify")]
        public async Task ItemIsDeleted()
        {
            var item = (await client.GetItemsAsync()).First();

            Assert.IsTrue(await client.DeleteAsync(item.Id));

            var items = (await client.GetItemsAsync()).ToList();
            CollectionAssert.DoesNotContain(items, item);
        }

        private async Task<WallabagItem> SetupSampleItem()
        {
            WallabagItem item = await client.AddAsync(
                uri: new Uri("https://jlnostr.de/blog/dokumente-schreiben-markdown-latex-pandoc"),
               tags: new string[] { "test", "markdown", "latex" });
            return item;
        }
    }
}
