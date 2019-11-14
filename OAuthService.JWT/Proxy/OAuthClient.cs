using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OAuthService.JWT.Proxy
{
    public class OAuthClient
    {
        private readonly HttpClient _httpClient;

        public OAuthClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JWTResult> EnsureApiTokenAsync(string username,
            string password,
            string secret)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", secret);

            HttpResponseMessage response = await _httpClient.PostAsync("/token", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "username", username },
                    { "password", password }
                }));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new JWTResult(true, JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
            }
            else
            {
                return new JWTResult(false, JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
            }

        }

        public async Task<JWTResult> RefreshTokenAsync(string refreshTokenId, string secret)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", secret);

            HttpResponseMessage response = await _httpClient.PostAsync("/token", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "refresh_token", refreshTokenId }
                }));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return new JWTResult(true, JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
            }
            else
            {
                return new JWTResult(false, JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
            }

        }
    }

    public class JWTResult
    {
        public JWTResult() { }

        public JWTResult(bool success, object result)
        {
            Success = success;
            Result = result;
        }

        public bool Success { get; set; }
        public object Result { get; set; }
    }
}
