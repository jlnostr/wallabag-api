using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace wallabag.Api
{
    public partial class WallabagClient : IWallabagClient
    {
        private HttpClient _httpClient;

        /// <summary>
        /// Gets or sets if exceptions should be thrown.
        /// </summary>
        public bool FireHtmlExceptions { get; set; }

        /// <summary>
        /// Gets the timeout in milliseconds for each HTTP request.
        /// </summary>
        public TimeSpan Timeout => _httpClient.Timeout;

        /// <summary>
        /// Initializes a new instance of WallabagClient.
        /// </summary>
        /// <param name="uri">The Uri of the wallabag instance of the user.</param>
        /// <param name="clientId">The OAuth client id of the app.</param>
        /// <param name="clientSecret">The OAuth client secret of the app.</param>
        /// <param name="timeout">Number in milliseconds after the request will be cancelled.</param>
        /// <param name="fireHtmlExceptions">Value that indicates if exceptions should be thrown.</param>
        public WallabagClient(
            Uri uri,
            string clientId,
            string clientSecret,
            int timeout = 0,
            bool fireHtmlExceptions = false)
        {
            InstanceUri = uri;
            ClientId = clientId;
            ClientSecret = clientSecret;

            if (!string.IsNullOrEmpty(AccessToken) && !string.IsNullOrEmpty(RefreshToken))
            {
                AccessToken = AccessToken;
                RefreshToken = RefreshToken;
            }

            _httpClient = new HttpClient();
            if (timeout > 0)
                _httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

            FireHtmlExceptions = fireHtmlExceptions;
        }

        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Returns the version number of the current wallabag instance.
        /// </summary>
        /// <returns>
        /// The version number of the server as string. Empty if it fails.
        /// </returns>
        public async Task<string> GetVersionNumberAsync()
        {
            var jsonString = await ExecuteHttpRequestAsync(HttpRequestMethod.Get, "/version", requiresAuthentication: false);
            return await ParseJsonFromStringAsync<string>(jsonString);
        }

        private async Task<string> ExecuteHttpRequestAsync(HttpRequestMethod httpRequestMethod, string relativeUriString, Dictionary<string, object> parameters = default(Dictionary<string, object>), bool requiresAuthentication = true)
        {
            var args = new PreRequestExecutionEventArgs();
            args.RequestMethod = httpRequestMethod;
            args.RequestUriSubString = relativeUriString;
            args.Parameters = parameters;
            PreRequestExecution?.Invoke(this, args);

            if (requiresAuthentication)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

                if (string.IsNullOrEmpty(AccessToken))
                    throw new Exception("Access token not available. Please create one using the RequestTokenAsync() method first.");
            }

            var uriString = $"{InstanceUri}api{relativeUriString}.json";

            if (httpRequestMethod == HttpRequestMethod.Get && parameters?.Count > 0)
            {
                uriString += "?";

                foreach (var item in parameters)
                    uriString += $"{item.Key}={item.Value.ToString()}&";

                // Remove the last ampersand (&).
                uriString = uriString.Remove(uriString.Length - 1);
            }

            Uri requestUri = new Uri(uriString);


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

            if (parameters != null && httpRequestMethod != HttpRequestMethod.Get)
                request.Content = new StringContent(JsonConvert.SerializeObject(parameters), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);
                AfterRequestExecution?.Invoke(this, response);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                    return null;
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine("[FAILURE] [wallabag-api] An error occured during the request: " + e.Message);

                if (FireHtmlExceptions)
                    throw e;

                return null;
            }
        }

        private Task<T> ParseJsonFromStringAsync<T>(string s)
        {
            if (!string.IsNullOrEmpty(s))
                return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(s));
            else
                return Task.FromResult(default(T));
        }

        /// <summary>
        /// The type of the HTTP request.
        /// </summary>
        public enum HttpRequestMethod { Delete, Get, Patch, Post, Put }

        /// <summary>
        /// Event that is fired before a HTTP request to the server is started.
        /// </summary>
        public event EventHandler<PreRequestExecutionEventArgs> PreRequestExecution;

        /// <summary>
        /// Event that is fired after the HTTP request is complete.
        /// </summary>
        public event EventHandler<HttpResponseMessage> AfterRequestExecution;
    }

    /// <summary>
    /// The arguments of the <see cref="WallabagClient.PreRequestExecution" /> event.
    /// </summary>
    public class PreRequestExecutionEventArgs
    {
        /// <summary>
        /// The substring that will attached to the <see cref="WallabagClient.InstanceUri"/> to perform a certain HTTP request.
        /// </summary>
        public string RequestUriSubString { get; set; }

        /// <summary>
        /// The type of the HTTP request.
        /// </summary>
        public WallabagClient.HttpRequestMethod RequestMethod { get; set; }

        /// <summary>
        /// Any parameters that are going to be submitted along with the request, e.g. the URL of a new item.
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }
    }

}
