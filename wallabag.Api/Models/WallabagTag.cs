using Newtonsoft.Json;

namespace wallabag.Api.Models
{
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
        public override bool Equals(object obj) => (obj as WallabagTag).Id.Equals(this.Id);
    }
}
