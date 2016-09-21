using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    public partial class GeneralTests
    {
        [TestMethod]
        [TestCategory("Tags")]
        public async Task TagsAreRetrieved()
        {
            List<WallabagTag> tags = (await client.GetTagsAsync()).ToList();
            Assert.IsTrue(tags.Count > 0);
        }

        [TestMethod]
        [TestCategory("Tags")]
        public async Task TagsAreAddedToItem()
        {
            var item = (await client.GetItemsAsync()).ToList().First();

            var tags = (await client.AddTagsAsync(item, new string[] { "wallabag", "test" })).ToList();
            CollectionAssert.AllItemsAreInstancesOfType(tags, typeof(WallabagTag));
            Assert.IsTrue(tags.Count >= 2);

            var modifiedItem = await client.GetItemAsync(item.Id);
            CollectionAssert.IsSubsetOf(tags, modifiedItem.Tags.ToList());
        }

        [TestMethod]
        [TestCategory("Tags")]
        public async Task TagsAreRemovedFromItem()
        {
            var item = (await client.GetItemsAsync()).ToList().First();
            Assert.IsTrue(await client.RemoveTagsAsync(item.Id, item.Tags.ToArray()));

            var modifiedItem = await client.GetItemAsync(item.Id);
            Assert.IsTrue(modifiedItem.Tags.Count() == 0);
        }


        [TestMethod]
        [TestCategory("Tags")]
        public async Task InvalidTagRemoveRequestReturnsFalse()
        {
            var item = (await client.GetItemsAsync()).ToList().First();

            var tags = new List<WallabagTag>();
            tags.Add(new WallabagTag() { Id = 12345, Label = "notexisting" });

            Assert.IsFalse(await client.RemoveTagsAsync(item.Id, tags));
        }


        [TestMethod]
        [TestCategory("Tags")]
        public async Task TagIsRemovedFromAllItems()
        {
            var tag = (await client.GetTagsAsync()).First();
            Assert.IsTrue(await client.RemoveTagFromAllItemsAsync(tag));

            var items = (await client.GetItemsAsync(tags: new string[] { tag.Label })).ToList();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        [TestCategory("Tags")]
        public async Task TagsAreRemovedFromAllItems()
        {
            var testTags = (await client.GetTagsAsync()).Take(2);
            var testTagsStringArray = testTags.ToCommaSeparatedString().Split(","[0]);

            var previousItems = (await client.GetItemsAsync(tags: testTagsStringArray)).ToList();

            Assert.IsTrue(await client.RemoveTagsFromAllItemsAsync(testTags));

            var newItems = (await client.GetItemsAsync(tags: testTagsStringArray)).ToList();
            Assert.IsTrue(newItems.Count == 0);        
        }
    }
}
