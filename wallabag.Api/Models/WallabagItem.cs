﻿using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;

namespace wallabag.Api.Models
{
    /// <summary>
    /// Item containing all available data.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class WallabagItem : IComparable
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the archived value.
        /// </summary>
        [JsonProperty("is_archived")]
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the starred value.
        /// </summary>
        [JsonProperty("is_starred")]
        public bool IsStarred { get; set; }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the date the item was added to wallabag.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the date the item was last updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the estimated reading time based on the calculations of wallabag.
        /// </summary>
        [JsonProperty("reading_time")]
        public int EstimatedReadingTime { get; set; }

        /// <summary>
        /// Gets or sets the domain name (the hostname).
        /// </summary>
        [JsonProperty("domain_name")]
        public string DomainName { get; set; }

        /// <summary>
        /// Gets or sets the media type (also known as mimetype).
        /// </summary>
        [JsonProperty("mimetype")]
        public string Mimetype { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }

        /// <summary>
        /// Gets or sets the tags of the items. <seealso cref="WallabagClient.AddTagsAsync(WallabagItem, IEnumerable{string})"/> and <seealso cref="WallabagClient.RemoveTagsAsync(WallabagItem, IEnumerable{WallabagTag})"/>.
        /// </summary>
        [JsonProperty("tags")]
        public IEnumerable<WallabagTag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the annotations of an item.
        /// </summary>
        [JsonProperty("annotations")]
        public List<WallabagAnnotation> Annotations { get; set; }

        /// <summary>
        /// Returns the HTTP status code that was returned when the item was saved.
        /// </summary>
        [JsonProperty("http_status")]
        public System.Net.HttpStatusCode? HttpStatusCode { get; }

        /// <summary>
        /// Gets or sets the Uri of the preview image.
        /// </summary>
        [JsonProperty("preview_picture")]
        public Uri PreviewImageUri { get; set; }

        /// <summary>
        /// Gets or sets the original article URL.
        /// </summary>
        [JsonProperty("origin_url")]
        public string OriginalUrl { get; set; }


        public override string ToString() => Title ?? string.Empty;
        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType().Equals(typeof(WallabagItem)))
            {
                var comparedItem = obj as WallabagItem;
                return Id.Equals(comparedItem.Id) && LastUpdated.Equals(comparedItem.LastUpdated);
            }
            return false;
        }
        public override int GetHashCode() => Id;
        public int CompareTo(object obj) => LastUpdated.CompareTo((obj as WallabagItem).LastUpdated);
    }
}
