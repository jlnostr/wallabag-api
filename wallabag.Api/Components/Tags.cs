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
        /// <summary>
        /// Returns a list of all available tags.
        /// </summary>
        public async Task<IEnumerable<WallabagTag>> GetTagsAsync()
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, "/tags");
            return await ParseJsonFromStringAsync<IEnumerable<WallabagTag>>(jsonString);
        }

        /// <summary>
        /// Adds tags to an item with a given id.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="tags">A string array of tags that should be added.</param>
        /// <returns>A list of all tags of the updated item with their specific id.</returns>
        public async Task<IEnumerable<WallabagTag>> AddTagsAsync(int itemId, string[] tags)
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Post, $"/entries/{itemId}/tags", new Dictionary<string, object>() { ["tags"] = tags.ToCommaSeparatedString() });
            var returnedItem = await ParseJsonFromStringAsync<WallabagItem>(jsonString);
            var itemTags = returnedItem?.Tags as List<WallabagTag>;

            // Check if the tags are in the returned item
            foreach (var item in tags)
                if (itemTags?.Where(t => t.Label == item).Count() != 1)
                    return default(IEnumerable<WallabagTag>);

            return returnedItem?.Tags;
        }

        /// <summary>
        /// Removes tags from an item with a given id.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="tags">An array of tags that should be removed.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> RemoveTagsAsync(int itemId, WallabagTag[] tags)
        {
            var lastJson = string.Empty;
            foreach (var item in tags)
                lastJson = await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/entries/{itemId}/tags/{item.Id}");

            var returnedItem = await ParseJsonFromStringAsync<WallabagItem>(lastJson);
            var itemTags = returnedItem?.Tags as List<WallabagTag>;

            // Check if the tags aren't no longer in the returned item
            foreach (var item in tags)
                if (itemTags?.Where(t => t.Label == item.Label).Count() == 1)
                    return false;

            return true;
        }

        /// <summary>
        /// Removes a tag from all items.
        /// </summary>
        /// <param name="tag">The tag that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> RemoveTagFromAllItemsAsync(WallabagTag tag)
        {
            await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/tags/{tag.Id}");
            return true;
        }

        /// <summary>
        /// Adds tags to an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">A string array of tags that should be added.</param>
        /// <returns>A list of all tags of the updated item with their specific id.</returns>
        public Task<IEnumerable<WallabagTag>> AddTagsAsync(WallabagItem item, string[] tags) => AddTagsAsync(item.Id, tags);

        /// <summary>
        /// Removes tags from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">An array of tags that should be removed.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> RemoveTagsAsync(WallabagItem item, WallabagTag[] tags) => RemoveTagsAsync(item.Id, tags);
    }
}
