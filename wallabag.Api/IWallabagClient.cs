using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.Models;
using static wallabag.Api.WallabagClient;

namespace wallabag.Api
{
    interface IWallabagClient : IDisposable
    {
        Uri InstanceUri { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }

        Task<string> GetVersionNumberAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RefreshAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RequestTokenAsync(string username, string password, CancellationToken cancellationToken = default(CancellationToken));

        Task<WallabagItem> AddAsync(Uri uri, IEnumerable<string> tags = null, string title = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<WallabagItem>> GetItemsAsync(
            bool? isRead = null,
            bool? isStarred = null,
            WallabagDateOrder? dateOrder = null,
            WallabagSortOrder? sortOrder = null,
            int? pageNumber = null,
            int? itemsPerPage = null,
            DateTime? since = null,
            IEnumerable<string> tags = null,
            CancellationToken cancellationToken = default(CancellationToken));
        Task<WallabagItem> GetItemAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<WallabagTag>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ArchiveAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> ArchiveAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> UnarchiveAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> UnarchiveAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> FavoriteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> FavoriteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> UnfavoriteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> UnfavoriteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> DeleteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<WallabagTag>> AddTagsAsync(int itemId, IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken));
        Task<IEnumerable<WallabagTag>> AddTagsAsync(WallabagItem item, IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveTagsAsync(int itemId, IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveTagsAsync(WallabagItem item, IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveTagFromAllItemsAsync(WallabagTag tag, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveTagFromAllItemsAsync(string tag, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken));
    }
}
