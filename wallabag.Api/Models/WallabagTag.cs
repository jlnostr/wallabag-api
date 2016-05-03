using Newtonsoft.Json;

namespace wallabag.Api.Models
{
    public class WallabagTag
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        public override string ToString() => this.Label;
        public override int GetHashCode() => this.Id;
        public override bool Equals(object obj) => (obj as WallabagTag).Id.Equals(this.Id);
    }
}
