using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        /// <summary>
        /// Returns a list of all available tags.
        /// </summary>
        public async Task<IEnumerable<WallabagTag>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
            => await ExecuteHttpRequestAsync<IEnumerable<WallabagTag>>(HttpRequestMethod.Get, BuildApiRequestUri("/tags"), cancellationToken);

        #region AddTagsAsync            
        /// <summary>
        /// Adds tags to an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">The tags that should be added.</param>
        /// <returns>A list of all tags of the updated item with their specific id.</returns>
        public Task<IEnumerable<WallabagTag>> AddTagsAsync(WallabagItem item, IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken))
            => AddTagsAsync(item.Id, tags, cancellationToken);

        /// <summary>
        /// Adds tags to an item.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="tags">The tags that should be added.</param>
        /// <returns>A list of all tags of the updated item with their specific id.</returns>
        public async Task<IEnumerable<WallabagTag>> AddTagsAsync(int itemId, IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            var returnedItem = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Post, BuildApiRequestUri($"/entries/{itemId}/tags"), cancellationToken, new Dictionary<string, object>() { ["tags"] = tags.ToCommaSeparatedString() });
            var itemTags = returnedItem?.Tags as List<WallabagTag>;

            // Check if the tags are in the returned item
            foreach (string item in tags)
                if (itemTags?.Where(t => t.Label == item).Count() != 1)
                    return default(IEnumerable<WallabagTag>);

            return returnedItem?.Tags;
        }

        #endregion

        #region RemoveTagsAsync

        /// <summary>
        /// Removes tags from an item with a given id.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="tags">An array of tags that should be removed.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> RemoveTagsAsync(int itemId, IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken))
        {
            WallabagItem lastItem = default(WallabagItem);

            foreach (var item in tags)
            {
                lastItem = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Delete, BuildApiRequestUri($"/entries/{itemId}/tags/{item.Id}"), cancellationToken);
                if (lastItem == null)
                    return false;
            }

            var itemTags = lastItem?.Tags as List<WallabagTag>;

            // Check if the tags aren't no longer in the returned item
            foreach (var item in tags)
                if (itemTags?.Where(t => t.Label == item.Label).Count() == 1)
                    return false;

            return true;
        }

        /// <summary>
        /// Removes tags from an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tags">An array of tags that should be removed.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> RemoveTagsAsync(WallabagItem item, IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken)) => RemoveTagsAsync(item.Id, tags, cancellationToken);

        #endregion

        /// <summary>
        /// Removes a tag from all items.
        /// </summary>
        /// <param name="tag">The tag that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> RemoveTagFromAllItemsAsync(WallabagTag tag, CancellationToken cancellationToken = default(CancellationToken))
            => RemoveTagFromAllItemsAsync(tag.Label, cancellationToken);

        /// <summary>
        /// Removes a tag from all items.
        /// </summary>
        /// <param name="tag">The tag that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> RemoveTagFromAllItemsAsync(string tag, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecuteHttpRequestAsync<object>(HttpRequestMethod.Delete, BuildApiRequestUri("/tag/label"), cancellationToken, new Dictionary<string, object>()
            {
                ["tag"] = tag
            }) != null;

        /// <summary>
        /// Removes one or more tags from all items.
        /// </summary>
        /// <param name="tags">A list with all the tags that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken))
            => RemoveTagsFromAllItemsAsync(tags.ToCommaSeparatedString().Split(","[0]), cancellationToken);

        /// <summary>
        /// Removes one or more tags from all items.
        /// </summary>
        /// <param name="tags">A list with all the tags that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken))
            => await ExecuteHttpRequestAsync<object>(HttpRequestMethod.Delete, BuildApiRequestUri("/tags/label"), cancellationToken,
                new Dictionary<string, object>()
                {
                    ["tags"] = tags.ToCommaSeparatedString()
                }) != null;
    }
}
