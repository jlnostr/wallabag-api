using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        /// <summary>
        /// Marks an item as read.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> ArchiveAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["archive"] = true.ToInt() });
            var item = await ParseJsonFromStringAsync<WallabagItem>(jsonString);

            return item?.IsRead == true;
        }

        /// <summary>
        /// Unmarks an item as read.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> UnarchiveAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["archive"] = false.ToInt() });
            var item = await ParseJsonFromStringAsync<WallabagItem>(jsonString);
            
            return item?.IsRead == false;
        }

        /// <summary>
        /// Marks an item as starred.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> FavoriteAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["starred"] = true.ToInt() });
            var item = await ParseJsonFromStringAsync<WallabagItem>(jsonString);

            return item?.IsStarred == true;
        }

        /// <summary>
        /// Unmarks an item as read.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> UnfavoriteAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["starred"] = false.ToInt() });
            var item = await ParseJsonFromStringAsync<WallabagItem>(jsonString);

            return item?.IsStarred == false;
        }

        /// <summary>
        /// Deletes an item PERMANENTLY.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> DeleteAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/entries/{itemId}");

            //TODO: Better check for actual result.
            return true;
        }

        /// <summary>
        /// Marks an item as read.
        /// </summary>
        /// <param name="item">The item that should be marked as read.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> ArchiveAsync(WallabagItem item) => ArchiveAsync(item.Id);

        /// <summary>
        /// Unmarks an item as read.
        /// </summary>
        /// <param name="item">The item that should be unmarked as read.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> UnarchiveAsync(WallabagItem item) => UnarchiveAsync(item.Id);

        /// <summary>
        /// Marks an item as starred.
        /// </summary>
        /// <param name="item">The item that should be marked as starred.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> FavoriteAsync(WallabagItem item) => FavoriteAsync(item.Id);

        /// <summary>
        /// Unmarks an item as starred.
        /// </summary>
        /// <param name="item">The item that should be unmarked as starred.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> UnfavoriteAsync(WallabagItem item) => UnfavoriteAsync(item.Id);

        /// <summary>
        /// Deletes an item PERMANENTLY.
        /// </summary>
        /// <param name="item">The item that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> DeleteAsync(WallabagItem item) => DeleteAsync(item.Id);
    }
}
