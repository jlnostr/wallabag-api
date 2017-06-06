using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.Models;
using wallabag.Api.Responses;

namespace wallabag.Api
{
    /// <summary>
    /// Represents an instance of WallabagClient which is used to access the API.
    /// </summary>
    public partial class WallabagClient
    {
        private Regex _languageCodeRegex = new Regex(@"^\w{2}(-\w{2})?");

        /// <summary>
        /// Returns a list of items filtered by given parameters.
        /// </summary>
        /// <param name="isRead">Indicates if the item is read (archived) or not.</param>
        /// <param name="isStarred">Indicates if the item is starred.</param>
        /// <param name="dateOrder">Sort order, in which the items should be returned. Can be <see cref="WallabagDateOrder.ByCreationDate"/> or <see cref="WallabagDateOrder.ByLastModificationDate"/>.</param>
        /// <param name="sortOrder">"Classic" sort order, ascending or descending.</param>
        /// <param name="pageNumber">Number of page.</param>
        /// <param name="itemsPerPage">Number of items per page.</param>
        /// <param name="since">Minimum timestamp that the creation date should have. Requires wallabag 2.1.</param>
        /// <param name="tags">An array of tags that applies to all items. Requires wallabag 2.1.</param>
        /// <returns></returns>
        public async Task<IEnumerable<WallabagItem>> GetItemsAsync(
            bool? isRead = null,
            bool? isStarred = null,
            WallabagDateOrder? dateOrder = null,
            WallabagSortOrder? sortOrder = null,
            int? pageNumber = null,
            int? itemsPerPage = null,
            DateTime? since = null,
            IEnumerable<string> tags = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await GetItemsWithEnhancedMetadataAsync(isRead, isStarred, dateOrder, sortOrder, pageNumber, itemsPerPage, since, tags, cancellationToken))?.Items;
        }

        /// <summary>
        /// Returns a result of <see cref="ItemCollectionResponse"/> that contains metadata (number of pages, current page, etc.) along with the items.
        /// </summary>
        /// <param name="isRead">Indicates if the item is read (archived) or not.</param>
        /// <param name="isStarred">Indicates if the item is starred.</param>
        /// <param name="dateOrder">Sort order, in which the items should be returned. Can be <see cref="WallabagDateOrder.ByCreationDate"/> or <see cref="WallabagDateOrder.ByLastModificationDate"/>.</param>
        /// <param name="sortOrder">"Classic" sort order, ascending or descending.</param>
        /// <param name="pageNumber">Number of page.</param>
        /// <param name="itemsPerPage">Number of items per page.</param>       
        /// <param name="since">Minimum timestamp that the creation date should have. Requires wallabag 2.1.</param>
        /// <param name="tags">An array of tags that applies to all items. Requires wallabag 2.1.</param>   
        /// <returns></returns>
        public async Task<ItemCollectionResponse> GetItemsWithEnhancedMetadataAsync(
            bool? isRead = null,
            bool? isStarred = null,
            WallabagDateOrder? dateOrder = null,
            WallabagSortOrder? sortOrder = null,
            int? pageNumber = null,
            int? itemsPerPage = null,
            DateTime? since = null,
            IEnumerable<string> tags = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            if (isRead != null)
                parameters.Add("archive", ((bool)isRead));
            if (isStarred != null)
                parameters.Add("starred", ((bool)isStarred));
            if (dateOrder != null)
                parameters.Add("sort", (dateOrder == WallabagDateOrder.ByCreationDate ? "created" : "updated"));
            if (sortOrder != null)
                parameters.Add("order", (sortOrder == WallabagSortOrder.Ascending ? "asc" : "desc"));
            if (pageNumber != null)
                parameters.Add("page", pageNumber);
            if (itemsPerPage != null)
                parameters.Add("perPage", itemsPerPage);
            if (since != null && (await GetVersionNumberAsync()).Contains("2.1"))
                parameters.Add("since", since.Value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            if (tags != null)
                parameters.Add("tags", System.Net.WebUtility.HtmlEncode(tags.ToCommaSeparatedString()));

            var response = await ExecuteHttpRequestAsync<ItemCollectionResponse>(HttpRequestMethod.Get, BuildApiRequestUri("/entries"), cancellationToken, parameters);

            if (response != null)
                foreach (var item in response.Items)
                    ValidateItemProperties(item);

            return response;
        }

        /// <summary>
        /// Returns an item by the given id.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <returns><see cref="WallabagItem"/></returns>
        public async Task<WallabagItem> GetItemAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await ExecuteHttpRequestAsync<WallabagItem>(HttpRequestMethod.Get, BuildApiRequestUri($"/entries/{itemId}"), cancellationToken);
            ValidateItemProperties(result);
            return result;
        }

        private void ValidateItemProperties(WallabagItem item)
        {
            if (item?.PreviewImageUri?.IsAbsoluteUri == false)
            {
                var itemUri = new Uri(item.Url);

                // First try: Create the Uri with the protocol from the url
                // Useful for image uris like: //img.abcde.fgh/my_image.png
                string uriWithProtocol = $"{itemUri.Scheme}:{item.PreviewImageUri.ToString()}";
                if (Uri.IsWellFormedUriString(uriWithProtocol, UriKind.Absolute))
                {
                    item.PreviewImageUri = new Uri(uriWithProtocol);
                    return;
                }

                // Second try: Append the uri to the hostname
                string itemHost = string.Empty;
                int pathAndQueryLength = itemUri.PathAndQuery.Length - 1;

                if (pathAndQueryLength == 0)
                    itemHost = itemUri.AbsoluteUri;
                else
                    itemHost = itemUri.AbsoluteUri.Remove(itemUri.AbsoluteUri.Length - pathAndQueryLength);

                Uri.TryCreate(itemHost + item.PreviewImageUri, UriKind.Absolute, out var result);
                if (result.IsWellFormedOriginalString())
                    item.PreviewImageUri = result;
                else
                    item.PreviewImageUri = null;
            }

            if (!string.IsNullOrEmpty(item?.Language) && !_languageCodeRegex.IsMatch(item?.Language))
                item.Language = null;
        }

        /// <summary>
        /// Represents the order by which the items should be sorted.
        /// </summary>
        public enum WallabagDateOrder
        {
            /// <summary>
            /// Sorts the items by creation date.
            /// </summary>
            ByCreationDate,
            /// <summary>
            /// Sorts the items by last modification date.
            /// </summary>
            ByLastModificationDate
        }

        /// <summary>
        /// Represents the sorting method.
        /// </summary>
        public enum WallabagSortOrder
        {
            /// <summary>
            /// Sorts the items ascending.
            /// </summary>
            Ascending,
            /// <summary>
            /// Sorts the items descending.
            /// </summary>
            Descending
        }
    }
}
