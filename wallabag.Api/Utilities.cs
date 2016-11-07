using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace wallabag.Api
{
    /// <summary>
    /// Some utilities that were used by this API.
    /// </summary>
    static class Utilities
    {
        /// <summary>
        /// Short version of <code>string.Join(",", list);</code>
        /// </summary>
        internal static string ToCommaSeparatedString<T>(this IEnumerable<T> list) => string.Join(",", list);

        /// <summary>
        /// Returns 0 if false, otherwise 1.
        /// </summary>
        internal static int ToInt(this bool input) => input ? 1 : 0;

        internal static Task<HttpResponseMessage> TryPostAsync(this HttpClient client, Uri requestUri, HttpContent content, bool throwExceptions)
        {
            return client.PostAsync(requestUri, content).ContinueWith<HttpResponseMessage>(task =>
            {
                if (task.Exception != null && throwExceptions)
                    throw task.Exception;
                if (task.Exception != null && throwExceptions == false)
                    return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
                else
                    return task.Result;
            });
        }
    }
}
