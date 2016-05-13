using System.Collections.Generic;

namespace wallabag.Api
{
    /// <summary>
    /// Some utilities that were used by this API.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Short version of <code>string.Join(",", list);</code>
        /// </summary>
        public static string ToCommaSeparatedString<T>(this IEnumerable<T> list) => string.Join(",", list);
        
        /// <summary>
        /// Returns 0 if false, otherwise 1.
        /// </summary>
        public static int ToInt(this bool input) => input ? 1 : 0;
    }
}
