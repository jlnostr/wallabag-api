using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using wallabag.Api.Responses;
using wallabag.Api.Models;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        public Task<IEnumerable<WallabagAnnotation>> GetAnnotationsAsync(WallabagItem item) => GetAnnotationsAsync(item.Id);
        public async Task<IEnumerable<WallabagAnnotation>> GetAnnotationsAsync(int itemId) => null;

        public Task<bool> AddAnnotationAsync(WallabagItem item, WallabagAnnotation annotation) => AddAnnotationAsync(item.Id, annotation);
        public async Task<bool> AddAnnotationAsync(int itemId, WallabagAnnotation annotation) => false;

        public Task<bool> UpdateAnnotationAsync(WallabagAnnotation oldAnnotation, WallabagAnnotation newAnnotation) => UpdateAnnotationAsync(oldAnnotation.Id, newAnnotation);
        public async Task<bool> UpdateAnnotationAsync(string oldAnnotationId, WallabagAnnotation newAnnotation) => false;

        public Task<bool> DeleteAnnotationAsync(WallabagAnnotation annotation) => DeleteAnnotationAsync(annotation.Id);
        public async Task<bool> DeleteAnnotationAsync(string annotationId) => false;
    }
}
