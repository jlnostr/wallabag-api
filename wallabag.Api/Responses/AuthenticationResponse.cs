using Newtonsoft.Json;

namespace wallabag.Api.Responses
{
    class AuthenticationResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
