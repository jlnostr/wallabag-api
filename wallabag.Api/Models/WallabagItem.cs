using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace wallabag.Api.Models
{
    public class WallabagItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("is_archived")]
        public bool IsRead { get; set; }

        [JsonProperty("is_starred")]
        public bool IsStarred { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("updated_at")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("reading_time")]
        public int EstimatedReadingTime { get; set; }

        [JsonProperty("domain_name")]
        public string DomainName { get; set; }

        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("tags")]
        public IEnumerable<WallabagTag> Tags { get; set; }

        [JsonProperty("preview_picture")]
        public string PreviewImageUri { get; set; } = string.Empty;


        public override string ToString() => this.Title;
        public override bool Equals(object obj)
        {
            var comparedItem = obj as WallabagItem;
            return Id.Equals(comparedItem.Id) && CreationDate.Equals(comparedItem.CreationDate);
        }
        public override int GetHashCode() => Id;
    }
}
