using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api.Tests
{
    [TestClass]
    public class TagsTests : TestBaseClass
    {
        public override async Task InitializeAsync()
        {
            var firstItem = (await Client.GetItemsAsync(itemsPerPage: 1)).First();
            await Client.AddTagsAsync(firstItem, new string[] {
                "tag1", "tag2", "tag3", "tag4", "tag5"
            });
        }

        [TestMethod]
        public async Task TagsAreRetrieved()
        {
            List<WallabagTag> tags = (await Client.GetTagsAsync()).ToList();
            Assert.IsTrue(tags.Count > 0);
        }

        [TestMethod]
        public async Task TagsAreAddedToItem()
        {
            var item = (await Client.GetItemsAsync()).ToList().First();

            var tags = (await Client.AddTagsAsync(item, new string[] { "wallabag", "test" })).ToList();
            CollectionAssert.AllItemsAreInstancesOfType(tags, typeof(WallabagTag));
            Assert.IsTrue(tags.Count >= 2);

            var modifiedItem = await Client.GetItemAsync(item.Id);
            CollectionAssert.IsSubsetOf(tags, modifiedItem.Tags.ToList());
        }

        [TestMethod]
        public async Task TagsAreRemovedFromItem()
        {
            var item = (await Client.GetItemsAsync()).ToList().First();
            Assert.IsTrue(await Client.RemoveTagsAsync(item.Id, item.Tags.ToArray()));

            var modifiedItem = await Client.GetItemAsync(item.Id);
            Assert.IsTrue(modifiedItem.Tags.Count() == 0);
        }

        [TestMethod]
        public async Task InvalidTagRemoveRequestReturnsFalse()
        {
            var item = (await Client.GetItemsAsync()).ToList().First();

            var tags = new List<WallabagTag>();
            tags.Add(new WallabagTag() { Id = 12345, Label = "notexisting" });

            Assert.IsFalse(await Client.RemoveTagsAsync(item.Id, tags));
        }

        [TestMethod]
        public async Task TagIsRemovedFromAllItems()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(1))
            {
                var tag = (await Client.GetTagsAsync()).First();
                Assert.IsTrue(await Client.RemoveTagFromAllItemsAsync(tag));

                var items = (await Client.GetItemsAsync(tags: new string[] { tag.Label })).ToList();
                Assert.IsTrue(items.Count == 0);
            }
        }

        [TestMethod]
        public async Task TagsAreRemovedFromAllItems()
        {
            if (await Client.MinorIsGreaterOrEqualAsync(1))
            {
                var testTags = (await Client.GetTagsAsync()).Take(2);
                var testTagsStringArray = testTags.ToCommaSeparatedString().Split(","[0]);

                var previousItems = (await Client.GetItemsAsync(tags: testTagsStringArray)).ToList();

                Assert.IsTrue(await Client.RemoveTagsFromAllItemsAsync(testTags));

                var newItems = (await Client.GetItemsAsync(tags: testTagsStringArray)).ToList();
                Assert.IsTrue(newItems.Count == 0);
            }
        }
    }
}
