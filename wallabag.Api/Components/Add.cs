using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        public async Task<WallabagItem> AddAsync(Uri uri, string[] tags = null, string title = null)
        {
            if (string.IsNullOrEmpty(uri.ToString()))
                throw new ArgumentNullException(nameof(uri));

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("url", uri);

            if (tags != null)
                parameters.Add("tags", tags.ToCommaSeparatedString());
            if (title != null)
                parameters.Add("title", title);

            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Post, "/entries", parameters);
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<WallabagItem>(jsonString));
        }
    }
}
