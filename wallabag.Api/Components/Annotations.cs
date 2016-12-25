using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        public Task<IEnumerable<WallabagAnnotation>> GetAnnotationsAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken))
            => GetAnnotationsAsync(item.Id, cancellationToken);
        public async Task<IEnumerable<WallabagAnnotation>> GetAnnotationsAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken))
        {
            string jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, $"/annotations/{itemId}", cancellationToken);
            var result = await ParseJsonFromStringAsync<WallabagAnnotationRoot>(jsonString, cancellationToken);

            return result.Annotations ?? new List<WallabagAnnotation>();
        }

        public Task<WallabagAnnotation> AddAnnotationAsync(WallabagItem item, WallabagAnnotation annotation, CancellationToken cancellationToken = default(CancellationToken))
            => AddAnnotationAsync(item.Id, annotation, cancellationToken);
        public async Task<WallabagAnnotation> AddAnnotationAsync(int itemId, WallabagAnnotation annotation, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, object>
            {
                { "ranges", annotation.Ranges },
                { "text", annotation.Text }
            };

            if (!string.IsNullOrEmpty(annotation.Quote))
                parameters.Add("quote", annotation.Quote);

            string jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Post, $"/annotations/{itemId}", cancellationToken, parameters);
            var result = await ParseJsonFromStringAsync<WallabagAnnotation>(jsonString, cancellationToken);

            return result;
        }

        public Task<WallabagAnnotation> UpdateAnnotationAsync(WallabagAnnotation oldAnnotation, WallabagAnnotation newAnnotation, CancellationToken cancellationToken = default(CancellationToken))
            => UpdateAnnotationAsync(oldAnnotation.Id, newAnnotation, cancellationToken);
        public async Task<WallabagAnnotation> UpdateAnnotationAsync(int oldAnnotationId, WallabagAnnotation newAnnotation, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, object>
            {
                { "ranges", newAnnotation.Ranges },
                { "text", newAnnotation.Text }
            };

            if (!string.IsNullOrEmpty(newAnnotation.Quote))
                parameters.Add("quote", newAnnotation.Quote);

            string jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Put, $"/annotations/{oldAnnotationId}", cancellationToken, parameters);
            var result = await ParseJsonFromStringAsync<WallabagAnnotation>(jsonString, cancellationToken);

            return result;
        }

        public Task<bool> DeleteAnnotationAsync(WallabagAnnotation annotation, CancellationToken cancellationToken = default(CancellationToken))
            => DeleteAnnotationAsync(annotation.Id, cancellationToken);
        public async Task<bool> DeleteAnnotationAsync(int annotationId, CancellationToken cancellationToken = default(CancellationToken))
            => !string.IsNullOrEmpty(await ExecuteHttpRequestAsync(HttpRequestMethod.Delete, $"/annotations/{annotationId}", cancellationToken));
    }
}
