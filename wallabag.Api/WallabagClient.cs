using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace wallabag.Api
{
    public partial class WallabagClient : IWallabagClient
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of WallabagClient.
        /// </summary>
        /// <param name="Uri">The Uri of the wallabag instance of the user.</param>
        /// <param name="ClientId">The OAuth client id of the app.</param>
        /// <param name="ClientSecret">The OAuth client secret of the app.</param>
        public WallabagClient(Uri Uri, string ClientId, string ClientSecret)
        {
            this.InstanceUri = Uri;
            this.ClientId = ClientId;
            this.ClientSecret = ClientSecret;

            this._httpClient = new HttpClient();
        }

        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Returns the version number of the current wallabag instance.
        /// </summary>
        public async Task<string> GetVersionNumberAsync()
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, "/version");
            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<string>(jsonString));
        }

        protected async Task<string> ExecuteHttpRequestAsync(HttpRequestMethod httpRequestMethod, string RelativeUriString, Dictionary<string, object> parameters = default(Dictionary<string, object>))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new HttpCredentialsHeaderValue("Bearer", await GetAccessTokenAsync());

            if (string.IsNullOrEmpty(AccessToken))
                throw new Exception("Access token not available. Please create one using the RequestTokenAsync() method first.");

            Uri requestUri = new Uri($"{InstanceUri}api{RelativeUriString}.json");
            var content = new HttpStringContent(JsonConvert.SerializeObject(parameters), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            string httpMethodString = "GET";
            switch (httpRequestMethod)
            {
                case HttpRequestMethod.Delete: httpMethodString = "DELETE"; break;
                case HttpRequestMethod.Patch: httpMethodString = "PATCH"; break;
                case HttpRequestMethod.Post: httpMethodString = "POST"; break;
                case HttpRequestMethod.Put: httpMethodString = "PUT"; break;
            }

            var method = new HttpMethod(httpMethodString);
            var request = new HttpRequestMessage(method, requestUri);

            if (parameters != null)
                request.Content = content;

            try
            {
                var response = await _httpClient.SendRequestAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex) { throw ex; }
        }
        public enum HttpRequestMethod { Delete, Get, Patch, Post, Put }

    }
}
