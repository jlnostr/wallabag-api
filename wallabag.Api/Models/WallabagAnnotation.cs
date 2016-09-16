using PropertyChanged;

namespace wallabag.Api.Models
{
    [ImplementPropertyChanged]
    public class WallabagAnnotation
    {
        public string Id { get; set; }

    }
}