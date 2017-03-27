using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.EventArgs;

namespace wallabag.Api
{
    public partial class WallabagClient : IWallabagClient
    {
        private HttpClient _httpClient;
        private string _version;

        /// <summary>
        /// Gets or sets if exceptions should be thrown.
        /// </summary>
        public bool ThrowHttpExceptions { get; set; }

        /// <summary>
        /// Gets the timeout in milliseconds for each HTTP request.
        /// </summary>
        public TimeSpan Timeout => _httpClient.Timeout;

        /// <summary>
        /// Returns the API version of the current instance. 
        /// Empty if not fetched yet via <see cref="GetVersionAsync(bool, CancellationToken)"/> or
        /// <see cref="GetVersionNumberAsync(bool, CancellationToken)"/>.
        /// </summary>
        public string ApiVersion { get => _version; }

        /// <summary>
        /// Initializes a new instance of WallabagClient.
        /// </summary>
        /// <param name="uri">The Uri of the wallabag instance of the user.</param>
        /// <param name="clientId">The OAuth client id of the app.</param>
        /// <param name="clientSecret">The OAuth client secret of the app.</param>
        /// <param name="timeout">Number in milliseconds after the request will be cancelled.</param>
        /// <param name="throwHttpExceptions">Value that indicates if exceptions should be thrown.</param>
        public WallabagClient(
            Uri uri,
            string clientId,
            string clientSecret,
            int timeout = 0,
            bool throwHttpExceptions = false)
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
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/json");

            if (timeout > 0)
                _httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

            ThrowHttpExceptions = throwHttpExceptions;
        }

        public void Dispose() => _httpClient.Dispose();

        /// <summary>
        /// Returns the version number of the current wallabag instance.
        /// </summary>
        /// <returns>
        /// The version number of the server as string. Empty if it fails.
        /// </returns>
        public async Task<string> GetVersionNumberAsync(bool forceRefresh = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(_version) || forceRefresh)
                _version = await ExecuteHttpRequestAsync<string>(HttpRequestMethod.Get, BuildApiRequestUri("/version"), cancellationToken, requiresAuthentication: false);

            return _version;
        }

        /// <summary>
        /// Returns the version number of the current wallabag instance as <see cref="ApiVersion"/>.
        /// </summary>
        public async Task<Version> GetVersionAsync(bool forceRefresh = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            string versionNumber = await GetVersionNumberAsync(forceRefresh, cancellationToken);

            Version.TryParse(versionNumber, out var result);

            return result;
        }

        /// <summary>
        /// Creates a valid relative Uri for <see cref="ExecuteHttpRequestAsync{T}(HttpRequestMethod, Uri, CancellationToken, Dictionary{string, object}, bool)"/>.
        /// </summary>
        /// <param name="s">The string.</param>
        private Uri BuildApiRequestUri(string s) => new Uri($"api{s}.json", UriKind.Relative);

        /// <summary>
        /// Execute a HTTP request and deserialize it immediately.
        /// </summary>
        /// <typeparam name="T">The object you want to fetch and that is the type of the deserialization.</typeparam>
        /// <param name="httpRequestMethod">The type of the http method.</param>
        /// <param name="requestSubUri">The relative string that is attached to the <see cref="InstanceUri"/>.</param>
        /// <param name="parameters">Parameters that should be submitted along with the request.</param>
        /// <param name="requiresAuthentication">Indicating, if the default Authorization header should be attached to the request.</param>
        /// <returns>If successful, the response as <typeparamref name="T"/>, otherwise the default value of <typeparamref name="T"/>.</returns>
        private async Task<T> ExecuteHttpRequestAsync<T>(
            HttpRequestMethod httpRequestMethod,
            Uri requestSubUri,
            CancellationToken cancellationToken,
            Dictionary<string, object> parameters = default(Dictionary<string, object>),
            bool requiresAuthentication = true)
        {
            // Before executing the request check if the cancellation was requested
            cancellationToken.ThrowIfCancellationRequested();

            // Execute the PreRequestExecution event
            var args = new PreRequestExecutionEventArgs()
            {
                RequestMethod = httpRequestMethod,
                RequestUriSubString = requestSubUri.ToString(),
                Parameters = parameters
            };
            PreRequestExecution?.Invoke(this, args);

            // Add the Authorization header if required
            if (requiresAuthentication)
            {
                if (string.IsNullOrEmpty(AccessToken))
                    throw new Exception("Access token not available. Please create one using the RequestTokenAsync() method first.");

                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync(cancellationToken));

                // The access token exists, but it's outdated and couldn't be updated due to several reasons.
                if (!string.IsNullOrEmpty(AccessToken) && DateTime.UtcNow.Subtract(LastTokenRefreshDateTime).TotalSeconds > 3600)
                    return default(T);
            }

            // Build the URI
            string requestUriString = new Uri(InstanceUri, requestSubUri).ToString();

            if (httpRequestMethod == HttpRequestMethod.Get && parameters?.Count > 0)
            {
                requestUriString += "?";

                foreach (var item in parameters)
                {
                    object value = item.Value;

                    if (item.Value is bool)
                        value = (bool)item.Value ? 1 : 0;

                    requestUriString += $"{item.Key}={value}&";
                }

                // Remove the last ampersand (&).
                requestUriString = requestUriString.Remove(requestUriString.Length - 1);
            }

            var requestUri = new Uri(requestUriString);

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
                request.Content = new StringContent(JsonConvert.SerializeObject(parameters, new JsonConverter[] { new Common.JsonBoolConverter() }), System.Text.Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            try
            {
                response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken);
                AfterRequestExecution?.Invoke(this, new AfterRequestExecutionEventArgs(args, response));

                if (response.IsSuccessStatusCode)
                    return await ParseJsonFromStreamAsync<T>(await response.Content.ReadAsStreamAsync(), cancellationToken);
                else
                    return default(T);
            }
            catch (HttpRequestException)
            {
                AfterRequestExecution?.Invoke(this, new AfterRequestExecutionEventArgs(args, response));
                if (ThrowHttpExceptions) throw;
                return default(T);
            }
        }

        private Task<T> ParseJsonFromStreamAsync<T>(Stream s, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                using (var st = s)
                using (var sr = new StreamReader(st))
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();

                    // read the json from a stream
                    // json size doesn't matter because only a small piece is read at a time from the HTTP request
                    return serializer.Deserialize<T>(reader);
                }
            }, cancellationToken);
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
        public event EventHandler<AfterRequestExecutionEventArgs> AfterRequestExecution;
    }
}
