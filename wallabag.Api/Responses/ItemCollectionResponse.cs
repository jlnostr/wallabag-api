using Newtonsoft.Json;
using System.Collections.Generic;
using wallabag.Api.Models;

namespace wallabag.Api.Responses
{
    public class ItemCollectionResponse
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("total")]
        public int TotalNumberOfItems { get; set; }

        [JsonProperty("_embedded")]
        public Embedded Embedded { get; set; }
    }
    public class Embedded
    {
        [JsonProperty("items")]
        public IEnumerable<WallabagItem> Items { get; set; }
    }
}
