using System;
using System.Collections.Generic;
using System.Threading;
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
        public async Task<bool> ArchiveAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var item = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Patch, BuildApiRequestUri($"/entries/{itemId}"), cancellationToken, new Dictionary<string, object>() { ["archive"] = true });

            return item?.IsRead == true;
        }

        /// <summary>
        /// Unmarks an item as read.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> UnarchiveAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var item = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Patch, BuildApiRequestUri($"/entries/{itemId}"), cancellationToken, new Dictionary<string, object>() { ["archive"] = false });

            return item?.IsRead == false;
        }

        /// <summary>
        /// Marks an item as starred.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> FavoriteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var item = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Patch, BuildApiRequestUri($"/entries/{itemId}"), cancellationToken, new Dictionary<string, object>() { ["starred"] = true });

            return item?.IsStarred == true;
        }

        /// <summary>
        /// Unmarks an item as read.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> UnfavoriteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var item = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Patch, BuildApiRequestUri($"/entries/{itemId}"), cancellationToken, new Dictionary<string, object>() { ["starred"] = false });

            return item?.IsStarred == false;
        }

        /// <summary>
        /// Deletes an item PERMANENTLY.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns>True, if the action was successful.</returns>
        public async Task<bool> DeleteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            await ExecuteHttpRequestAsync<object>(HttpRequestMethod.Delete, BuildApiRequestUri($"/entries/{itemId}"), cancellationToken);

            //TODO: Better check for actual result.
            return true;
        }

        /// <summary>
        /// Marks an item as read.
        /// </summary>
        /// <param name="item">The item that should be marked as read.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> ArchiveAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken)) => ArchiveAsync(item.Id, cancellationToken);

        /// <summary>
        /// Unmarks an item as read.
        /// </summary>
        /// <param name="item">The item that should be unmarked as read.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> UnarchiveAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken)) => UnarchiveAsync(item.Id, cancellationToken);

        /// <summary>
        /// Marks an item as starred.
        /// </summary>
        /// <param name="item">The item that should be marked as starred.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> FavoriteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken)) => FavoriteAsync(item.Id, cancellationToken);

        /// <summary>
        /// Unmarks an item as starred.
        /// </summary>
        /// <param name="item">The item that should be unmarked as starred.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> UnfavoriteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken)) => UnfavoriteAsync(item.Id, cancellationToken);

        /// <summary>
        /// Deletes an item PERMANENTLY.
        /// </summary>
        /// <param name="item">The item that should be deleted.</param>
        /// <returns>True, if the action was successful.</returns>
        public Task<bool> DeleteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken)) => DeleteAsync(item.Id, cancellationToken);
    }
}
