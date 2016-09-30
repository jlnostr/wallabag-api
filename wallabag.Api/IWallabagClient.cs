﻿using System;
using System.Collections.Generic;
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

        Task<string> GetVersionNumberAsync();

        Task<string> GetAccessTokenAsync();
        Task<bool> RefreshAccessTokenAsync();
        Task<bool> RequestTokenAsync(string username, string password);

        Task<WallabagItem> AddAsync(Uri uri, IEnumerable<string> tags = null, string title = null);

        Task<IEnumerable<WallabagItem>> GetItemsAsync(
            bool? isRead = null,
            bool? isStarred = null,
            WallabagDateOrder? dateOrder = null,
            WallabagSortOrder? sortOrder = null,
            int? pageNumber = null,
            int? itemsPerPage = null,
            DateTime? since = null,
            IEnumerable<string> tags = null);
        Task<WallabagItem> GetItemAsync(int itemId);
        Task<IEnumerable<WallabagTag>> GetTagsAsync();

        Task<bool> ArchiveAsync(int itemId);
        Task<bool> ArchiveAsync(WallabagItem item);
        Task<bool> UnarchiveAsync(int itemId);
        Task<bool> UnarchiveAsync(WallabagItem item);
        Task<bool> FavoriteAsync(int itemId);
        Task<bool> FavoriteAsync(WallabagItem item);
        Task<bool> UnfavoriteAsync(int itemId);
        Task<bool> UnfavoriteAsync(WallabagItem item);
        Task<bool> DeleteAsync(int itemId);
        Task<bool> DeleteAsync(WallabagItem item);

        Task<IEnumerable<WallabagTag>> AddTagsAsync(int itemId, IEnumerable<string> tags);
        Task<IEnumerable<WallabagTag>> AddTagsAsync(WallabagItem item, IEnumerable<string> tags);
        Task<bool> RemoveTagsAsync(int itemId, IEnumerable<WallabagTag> tags);
        Task<bool> RemoveTagsAsync(WallabagItem item, IEnumerable<WallabagTag> tags);
        Task<bool> RemoveTagFromAllItemsAsync(WallabagTag tag);
        Task<bool> RemoveTagFromAllItemsAsync(string tag);
        Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<WallabagTag> tags);
        Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<string> tags);        
    }
}
