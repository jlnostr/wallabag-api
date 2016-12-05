using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using wallabag.Api.Responses;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        /// <summary>
        /// The Uri of the wallabag instance.
        /// </summary>
        public Uri InstanceUri { get; set; }
        private Uri _AuthenticationUri { get { return new Uri($"{InstanceUri}oauth/v2/token"); } }

        /// <summary>
        /// The DateTime value that specifies the last execution method of <see cref="RefreshAccessTokenAsync"/>.
        /// </summary>
        public DateTime LastTokenRefreshDateTime { get; set; }

        /// <summary>
        /// The given client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The given client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The given access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The given refresh token.
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Requests both <see cref="AccessToken"/> and <see cref="RefreshToken"/> for the first time. 
        /// Should be used only once, for future authentication calls use <see cref="RefreshAccessTokenAsync"/>.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True, if login was successful, false otherwise.</returns>
        public async Task<bool> RequestTokenAsync(string username, string password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            parameters.Add("client_id", ClientId);
            parameters.Add("client_secret", ClientSecret);
            parameters.Add("username", username);
            parameters.Add("password", password);

            var content = new StringContent(JsonConvert.SerializeObject(parameters), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.TryPostAsync(_AuthenticationUri, content, ThrowHttpExceptions);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseString = await response.Content.ReadAsStringAsync();

            var result = await ParseJsonFromStringAsync<AuthenticationResponse>(responseString);
            AccessToken = result.AccessToken;
            RefreshToken = result.RefreshToken;

            LastTokenRefreshDateTime = DateTime.UtcNow;

            return true;
        }

        /// <summary>
        /// Returns always a valid <see cref="AccessToken"/>. In case it's expired, the <see cref="RefreshAccessTokenAsync"/> task is called.
        /// </summary>
        /// <returns>A valid <seealso cref="AccessToken"/>.</returns>
        public async Task<string> GetAccessTokenAsync()
        {
            TimeSpan duration = DateTime.UtcNow.Subtract(LastTokenRefreshDateTime);
            if (duration.TotalSeconds > 3600)
                await RefreshAccessTokenAsync();

            return AccessToken;
        }

        /// <summary>
        /// Refreshes a token by using the given <see cref="RefreshToken"/>.
        /// </summary>
        /// <returns>True, if re-authentication was successful, false otherwise.</returns>
        public async Task<bool> RefreshAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(RefreshToken))
                throw new ArgumentNullException("RefreshToken has no value. It will created once you've authenticated the first time.");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("client_id", ClientId);
            parameters.Add("client_secret", ClientSecret);
            parameters.Add("refresh_token", RefreshToken);

            var content = new StringContent(JsonConvert.SerializeObject(parameters), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.TryPostAsync(_AuthenticationUri, content, ThrowHttpExceptions);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseString = await response.Content.ReadAsStringAsync();

            var result = await ParseJsonFromStringAsync<AuthenticationResponse>(responseString);
            AccessToken = result.AccessToken;
            RefreshToken = result.RefreshToken;
            LastTokenRefreshDateTime = DateTime.UtcNow;

            CredentialsRefreshed?.Invoke(this, null);
            return true;
        }

        /// <summary>
        /// Event that will fired if <see cref="RefreshAccessTokenAsync"/> was successful.
        /// </summary>
        public event EventHandler CredentialsRefreshed;
    }
}
