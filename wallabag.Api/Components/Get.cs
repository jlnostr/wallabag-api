using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wallabag.Api.Models;
using wallabag.Api.Responses;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        /// <summary>
        /// Returns a list of items filtered by given parameters.
        /// </summary>
        /// <param name="IsRead">Indicates if the item is read (archived) or not.</param>
        /// <param name="IsStarred">Indicates if the item is starred.</param>
        /// <param name="DateOrder">Sort order, in which the items should be returned. Can be <see cref="WallabagDateOrder.ByCreationDate"/> or <see cref="WallabagDateOrder.ByLastModificationDate"/>.</param>
        /// <param name="SortOrder">"Classic" sort order, ascending or descending.</param>
        /// <param name="PageNumber">Number of page.</param>
        /// <param name="ItemsPerPage">Number of items per page.</param>
        /// <param name="Tags">An array of tags that applies to all items.</param>
        /// <returns></returns>
        public async Task<IEnumerable<WallabagItem>> GetItemsAsync(
            bool? IsRead = null,
            bool? IsStarred = null,
            WallabagDateOrder? DateOrder = null,
            WallabagSortOrder? SortOrder = null,
            int? PageNumber = null,
            int? ItemsPerPage = null,
            string[] Tags = null)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            var requestUriSubString = "/entries";

            if (IsRead != null)
                parameters.Add("archive", ((bool)IsRead).ToInt());
            if (IsStarred != null)
                parameters.Add("starred", ((bool)IsStarred).ToInt());
            if (DateOrder != null)
                parameters.Add("sort", (DateOrder == WallabagDateOrder.ByCreationDate ? "created" : "updated"));
            if (SortOrder != null)
                parameters.Add("order", (SortOrder == WallabagSortOrder.Ascending ? "asc" : "desc"));
            if (PageNumber != null)
                parameters.Add("page", PageNumber);
            if (ItemsPerPage != null)
                parameters.Add("perPage", ItemsPerPage);
            if (Tags != null)
                parameters.Add("tags", System.Net.WebUtility.HtmlEncode(Tags.ToCommaSeparatedString()));

            if (parameters.Count > 0)
            {
                requestUriSubString += "?";

                foreach (var item in parameters)
                    requestUriSubString += $"{item.Key}={item.Value.ToString()}&";

                // Remove the last ampersand (&).
                requestUriSubString = requestUriSubString.Remove(requestUriSubString.Length - 1);
            }

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, requestUriSubString);
            return (await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ItemCollectionResponse>(jsonString))).Embedded.Items;
        }

        /// <summary>
        /// Returns an item by the given id.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns><see cref="WallabagItem"/></returns>
        public async Task<WallabagItem> GetItemAsync(int itemId)
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, $"/entries/{itemId}");
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));
        }

        public enum WallabagDateOrder { ByCreationDate, ByLastModificationDate }
        public enum WallabagSortOrder { Ascending, Descending }
    }
}
