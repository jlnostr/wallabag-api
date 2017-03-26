using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
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
    }
}
