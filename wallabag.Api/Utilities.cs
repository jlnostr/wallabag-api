using System.Collections.Generic;

namespace wallabag.Api
{
    public static class Utilities
    {
        public static string ToCommaSeparatedString<T>(this IEnumerable<T> list) => string.Join(",", list);
        public static int ToInt(this bool input) => input ? 1 : 0;
    }
}
