using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;

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

    /// <summary>
    /// A class that represents an annotation.
    /// </summary>
    [ImplementPropertyChanged]
    public class WallabagAnnotation
    {
        /// <summary>
        /// The ID of the annotation.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// The schema version of the annotation.
        /// </summary>
        [JsonProperty("annotator_schema_version")]
        public string AnnotatorSchemaVersion { get; set; }

        /// <summary>
        /// The quoted text of the annotation.
        /// </summary>
        [JsonProperty("quote")]
        public string Quote { get; set; }

        /// <summary>
        /// The creation date of the annotation.
        /// </summary>
        [JsonProperty("created_at")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The text of the annotation.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// The last time where the annotation was updated.
        /// </summary>
        [JsonProperty("updated_at")]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// A list of <see cref="WallabagAnnotationRange"/> that defines where the annotation should be applied.
        /// </summary>
        [JsonProperty("ranges")]
        public IList<WallabagAnnotationRange> Ranges { get; set; } = new List<WallabagAnnotationRange>();

        /// <summary>
        /// The first item of <see cref="Ranges"/>, if there is exactly one item. Otherwise null.
        /// </summary>
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

        /// <summary>
        /// Creates a new annotation.
        /// </summary>
        public WallabagAnnotation() { }

        /// <summary>
        /// Creates a new annotation with one specific range.
        /// </summary>
        /// <param name="range">The range for the new annotation.</param>
        /// <param name="text">The text for the new annotation.</param>
        /// <param name="quote">The quoted text for the new annotation.</param>
        public WallabagAnnotation(WallabagAnnotationRange range, string text, string quote = "") : this()
        {
            Ranges.Add(range);
            Text = text;
            Quote = quote;
        }

        /// <summary>
        /// Creates a new annotation with one or more ranges.
        /// </summary>
        /// <param name="ranges">A list of ranges for the new annotation.</param>
        /// <param name="text">The text for the new annotation.</param>
        /// <param name="quote">The quoted text for the new annotation.</param>
        public WallabagAnnotation(IList<WallabagAnnotationRange> ranges, string text) : this()
        {
            Ranges = ranges;
            Text = text;
        }

        public override bool Equals(object obj) => (obj != null) && ((obj as WallabagAnnotation).Id.Equals(Id));
        public override int GetHashCode() => Id;
        public override string ToString() => Text;
    }

    /// <summary>
    /// A class that defines a range for an annotation.
    /// </summary>
    [ImplementPropertyChanged]
    public class WallabagAnnotationRange
    {
        /// <summary>
        /// The start element of the annotation. Expected style: /<tag-name>[index]
        /// </summary>
        [JsonProperty("start")]
        public string Start { get; set; }

        /// <summary>
        /// The start offset inside <see cref="Start"/>.
        /// </summary>
        [JsonProperty("startOffset")]
        public int StartOffset { get; set; }

        /// <summary>
        /// The end element of the annotation. Expected style: /<tag-name>[index]
        /// </summary>
        [JsonProperty("end")]
        public string End { get; set; }

        /// <summary>
        /// The end offset inside <see cref="End"/>.
        /// </summary>
        [JsonProperty("endOffset")]
        public int EndOffset { get; set; }

        /// <summary>
        /// Creates a new range.
        /// </summary>
        public WallabagAnnotationRange() { }

        /// <summary>
        /// Creates a new range with the given values.
        /// </summary>
        /// <param name="start">The start element of the annotation. Expected style: /<tag-name>[index]</param>
        /// <param name="startOffset">The start offset inside <see cref="Start"/>.</param>
        /// <param name="end">The end element of the annotation. Expected style: /<tag-name>[index]</param>
        /// <param name="endOffset">The end offset inside <see cref="End"/>.</param>
        public WallabagAnnotationRange(string start, int startOffset, string end, int endOffset)
        {
            Start = start;
            StartOffset = startOffset;
            End = end;
            EndOffset = endOffset;
        }
    }
}