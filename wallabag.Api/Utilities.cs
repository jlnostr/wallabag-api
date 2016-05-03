using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wallabag.Api
{
    public static class Utilities
    {
        public static string ToCommaSeparatedString<T>(this IEnumerable<T> list)
        {
            string result = string.Empty;
            if (list != null && list.Count() > 0)
            {
                List<string> tempList = new List<string>();
                foreach (var item in list)
                    tempList.Add(item.ToString());

                // The usage of Distinct avoids duplicates in the list.
                // If the type isn't string, it won't work.
                List<string> distinctList = tempList.Distinct().ToList();

                foreach (var item in distinctList)
                {
                    if (!string.IsNullOrWhiteSpace(item.ToString()))
                        result += item.ToString() + ",";
                }
                if (result.EndsWith(","))
                    result = result.Remove(result.Length - 1);
            }

            return result;
        }

        public static int ToInt(this bool input) => input ? 1 : 0;
    }
}
