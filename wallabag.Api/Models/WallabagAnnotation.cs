using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;

namespace wallabag.Api.Models
{
    [ImplementPropertyChanged]
    internal class WallabagAnnotationRoot
    {
        [JsonProperty("total")]
        public int Count { get; set; }

        [JsonProperty("rows")]
        public IEnumerable<WallabagAnnotation> Annotations { get; set; }
    }

    [ImplementPropertyChanged]
    public class WallabagAnnotation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("annotator_schema_version")]
        public string AnnotatorSchemaVersion { get; set; }

        [JsonProperty("quote")]
        public string Quote { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("updated_at")]
        public DateTime LastUpdated { get; set; }

        [JsonProperty("ranges")]
        public IList<WallabagAnnotationRange> Ranges { get; set; } = new List<WallabagAnnotationRange>();

        public WallabagAnnotationRange Range
        {
            get
            {
                if (Ranges.Count == 1)
                    return Ranges[0];
                else
                    return null;
            }
        }

        public WallabagAnnotation() { }
        public WallabagAnnotation(WallabagAnnotationRange range, string text, string quote = "") : this()
        {
            Ranges.Add(range);
            Text = text;
            Quote = quote;
        }
        public WallabagAnnotation(IList<WallabagAnnotationRange> ranges, string text, string quote = "") : this()
        {
            Ranges = ranges;
            Text = text;
            Quote = quote;
        }

        public override bool Equals(object obj) => (obj != null) && ((obj as WallabagAnnotation).Id.Equals(Id));
        public override int GetHashCode() => Id;
        public override string ToString() => Text;
    }

    [ImplementPropertyChanged]
    public class WallabagAnnotationRange
    {
        [JsonProperty("start")]
        public string Start { get; set; }

        [JsonProperty("startOffset")]
        public int StartOffset { get; set; }

        [JsonProperty("end")]
        public string End { get; set; }

        [JsonProperty("endOffset")]
        public int EndOffset { get; set; }

        public WallabagAnnotationRange() { }
        public WallabagAnnotationRange(string start, int startOffset, string end, int endOffset)
        {
            Start = start;
            StartOffset = startOffset;
            End = end;
            EndOffset = endOffset;
        }
    }
}