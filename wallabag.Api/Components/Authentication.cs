using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace wallabag.Api
{
    public partial class WallabagClient
    {
        public Uri InstanceUri { get; set; }
        public Uri _AuthenticationUri { get { return new Uri($"{InstanceUri}oauth/v2/token"); } }

        protected DateTime _LastRequestDateTime;

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public async Task<bool> RequestTokenAsync(string username, string password)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            parameters.Add("client_id", ClientId);
            parameters.Add("client_secret", ClientSecret);
            parameters.Add("username", username);
            parameters.Add("password", password);

            var content = new HttpStringContent(JsonConvert.SerializeObject(parameters), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            var response = await _httpClient.PostAsync(_AuthenticationUri, content);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            AccessToken = result.access_token;
            RefreshToken = result.refresh_token;

            _LastRequestDateTime = DateTime.UtcNow;

            return true;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            TimeSpan duration = DateTime.UtcNow.Subtract(_LastRequestDateTime);
            if (duration.TotalSeconds > 3600)
                await RefreshAccessTokenAsync();

            return AccessToken;
        }
        public async Task<bool> RefreshAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(RefreshToken))
                throw new Exception("RefreshToken has no value. It will created once you've authenticated the first time.");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("client_id", ClientId);
            parameters.Add("client_secret", ClientSecret);
            parameters.Add("refresh_token", RefreshToken);

            var content = new HttpStringContent(JsonConvert.SerializeObject(parameters), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            var response = await _httpClient.PostAsync(_AuthenticationUri, content);

            if (!response.IsSuccessStatusCode)
                return false;

            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            AccessToken = result.access_token;
            RefreshToken = result.refresh_token;
            _LastRequestDateTime = DateTime.UtcNow;

            return true;
        }
    }
}
