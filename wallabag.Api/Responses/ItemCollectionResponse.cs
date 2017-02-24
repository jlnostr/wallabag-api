using Newtonsoft.Json;
using System.Collections.Generic;
using wallabag.Api.Models;

namespace wallabag.Api.Responses
{
    /// <summary>
    /// Represents a class that contains metadata along with the items that were requested through the API.
    /// </summary>
    public class ItemCollectionResponse
    {
        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the number of total available pages with the current <see cref="Limit"/>.
        /// </summary>
        [JsonProperty("pages")]
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the limit of items per page.
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets the total number of items that were saved by the user.
        /// </summary>
        [JsonProperty("total")]
        public int TotalNumberOfItems { get; set; }

        [JsonProperty("_embedded")]
        private Embedded _Embedded { get; set; }

        /// <summary>
        /// Gets or sets the items that matches the current request.
        /// </summary>
        [JsonIgnore]
        public IEnumerable<WallabagItem> Items
        {
            get { return _Embedded?.Items; }
            set
            {
                if (_Embedded == null)
                    _Embedded = new Embedded();

                _Embedded.Items = value;
            }
        }
    }
    class Embedded
    {
        [JsonProperty("items")]
        public IEnumerable<WallabagItem> Items { get; set; }
    }
}
