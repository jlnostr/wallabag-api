using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.Responses;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        private Uri _authenticationUri = new Uri("oauth/v2/token", UriKind.Relative);

        /// <summary>
        /// The Uri of the wallabag instance.
        /// </summary>
        public Uri InstanceUri { get; set; }

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
        public async Task<bool> RequestTokenAsync(string username, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            var parameters = new Dictionary<string, object>
            {
                { "grant_type", "password" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "username", username },
                { "password", password }
            };

            var result = await ExecuteHttpRequestAsync<AuthenticationResponse>(HttpRequestMethod.Post, _authenticationUri, cancellationToken, parameters, false);

            if (result != null)
            {
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                LastTokenRefreshDateTime = DateTime.UtcNow;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns always a valid <see cref="AccessToken"/>. In case it's expired, the <see cref="RefreshAccessTokenAsync"/> task is called.
        /// </summary>
        /// <returns>A valid <seealso cref="AccessToken"/>.</returns>
        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var duration = DateTime.UtcNow.Subtract(LastTokenRefreshDateTime);
            if (duration.TotalSeconds > 3600)
                await RefreshAccessTokenAsync(cancellationToken);

            return AccessToken;
        }

        /// <summary>
        /// Refreshes a token by using the given <see cref="RefreshToken"/>.
        /// </summary>
        /// <returns>True, if re-authentication was successful, false otherwise.</returns>
        public async Task<bool> RefreshAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(RefreshToken))
                throw new ArgumentNullException("RefreshToken has no value. It will created once you've authenticated the first time.");

            var parameters = new Dictionary<string, object>
            {
                { "grant_type", "refresh_token" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "refresh_token", RefreshToken }
            };

            var result = await ExecuteHttpRequestAsync<AuthenticationResponse>(HttpRequestMethod.Post, _authenticationUri, cancellationToken, parameters, false);

            if (result != null)
            {
                AccessToken = result.AccessToken;
                RefreshToken = result.RefreshToken;
                LastTokenRefreshDateTime = DateTime.UtcNow;

                CredentialsRefreshed?.Invoke(this, null);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Event that will fired if <see cref="RefreshAccessTokenAsync"/> was successful.
        /// </summary>
        public event EventHandler CredentialsRefreshed;
    }
}
