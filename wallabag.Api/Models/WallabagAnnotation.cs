using PropertyChanged;
using System;

namespace wallabag.Api.Models
{
    [ImplementPropertyChanged]
    public class WallabagAnnotation
    {
        public int Id { get; set; }
        public string AnnotatorSchemaVersion { get; set; }
        public string Quote { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public DateTime LastUpdated { get; set; }
        public WallabagAnnotationRange Range { get; set; }
    }

    [ImplementPropertyChanged]
    public class WallabagAnnotationRange
    {
        public string Start { get; set; }
        public int StartOffset { get; set; }
        public string End { get; set; }
        public int EndOffset { get; set; }
    }
}