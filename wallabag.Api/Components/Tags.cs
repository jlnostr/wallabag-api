using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        public async Task<IEnumerable<WallabagTag>> GetTagsAsync()
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, "/tags");
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<IEnumerable<WallabagTag>>(jsonString));
        }

        public async Task<IEnumerable<WallabagTag>> AddTagsAsync(int itemId, string[] tags)
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Post, $"/entries/{itemId}/tags", new Dictionary<string, object>() { ["tags"] = tags.ToCommaSeparatedString() });
            var returnedItem = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));
            var itemTags = returnedItem.Tags as List<WallabagTag>;

            // Check if the tags are in the returned item
            foreach (var item in tags)
                if (itemTags.Where(t => t.Label == item).Count() != 1)
                    return default(IEnumerable<WallabagTag>);

            return returnedItem.Tags;
        }
        public async Task<bool> RemoveTagsAsync(int itemId, WallabagTag[] tags)
        {
            var lastJson = string.Empty;
            foreach (var item in tags)
                lastJson = await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/entries/{itemId}/tags/{item.Id}");

            var returnedItem = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(lastJson));
            var itemTags = returnedItem.Tags as List<WallabagTag>;

            // Check if the tags aren't in the returned item
            foreach (var item in tags)
                if (itemTags.Where(t => t.Label == item.Label).Count() == 1)
                    return false;

            return true;
        }
        public async Task<bool> RemoveTagFromAllItemsAsync(WallabagTag tag)
        {
            await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/tags/{tag.Id}");
            return true;
        }

        public Task<IEnumerable<WallabagTag>> AddTagsAsync(WallabagItem item, string[] tags) => AddTagsAsync(item.Id, tags);
        public Task<bool> RemoveTagsAsync(WallabagItem item, WallabagTag[] tags) => RemoveTagsAsync(item.Id, tags);
    }
}
