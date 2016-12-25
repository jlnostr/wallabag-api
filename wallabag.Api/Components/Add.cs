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
        /// Adds an item by given parameters.
        /// </summary>
        /// <param name="uri">The URL of the item you want to save.</param>
        /// <param name="tags">The tags that should be added to the item.</param>
        /// <param name="title">The title that should be given. Can be useful on certain cases, where items doesn't have a title, for example on PDF documents.</param>
        /// <returns></returns>
        public async Task<WallabagItem> AddAsync(Uri uri, IEnumerable<string> tags = null, string title = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(uri.ToString()))
                throw new ArgumentNullException(nameof(uri));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("url", uri);

            if (tags != null)
                parameters.Add("tags", tags.ToCommaSeparatedString());
            if (title != null)
                parameters.Add("title", title);

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Post, "/entries", cancellationToken, parameters);
            return await ParseJsonFromStringAsync<WallabagItem>(jsonString, cancellationToken);
        }
    }
}
