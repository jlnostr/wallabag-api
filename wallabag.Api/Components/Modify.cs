using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        public async Task<bool> ArchiveAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["archive"] = true.ToInt() });
            var item = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));

            return item.IsRead == true;
        }
        public async Task<bool> UnarchiveAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["archive"] = false.ToInt() });
            var item = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));

            return item.IsRead == false;
        }
        public async Task<bool> FavoriteAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["starred"] = true.ToInt() });
            var item = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));

            return item.IsStarred == true;
        }
        public async Task<bool> UnfavoriteAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Patch, $"/entries/{itemId}", new Dictionary<string, object>() { ["starred"] = false.ToInt() });
            var item = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));

            return item.IsStarred == false;
        }
        public async Task<bool> DeleteAsync(int itemId)
        {
            if (itemId == 0)
                throw new ArgumentNullException(nameof(itemId));

            await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/entries/{itemId}");
            return true;
        }

        public Task<bool> ArchiveAsync(WallabagItem item) => ArchiveAsync(item.Id);
        public Task<bool> UnarchiveAsync(WallabagItem item) => UnarchiveAsync(item.Id);
        public Task<bool> FavoriteAsync(WallabagItem item) => FavoriteAsync(item.Id);
        public Task<bool> UnfavoriteAsync(WallabagItem item) => UnfavoriteAsync(item.Id);
        public Task<bool> DeleteAsync(WallabagItem item) => DeleteAsync(item.Id);
    }
}
