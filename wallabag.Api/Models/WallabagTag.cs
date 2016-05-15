using Newtonsoft.Json;

namespace wallabag.Api.Models
{
    /// <summary>
    /// Represents a tag that can be applied to one or more items.
    /// </summary>
    public class WallabagTag
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the label of a tag.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the slug of the tag.
        /// </summary>
        [JsonProperty("slug")]
        public string Slug { get; set; }

        public override string ToString() => this.Label;
        public override int GetHashCode() => this.Id;
        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType().Equals(typeof(WallabagTag)))
            {
                var comparedItem = obj as WallabagTag;
                return Id.Equals(comparedItem.Id);
            }
            return false;
        }
    }
}
