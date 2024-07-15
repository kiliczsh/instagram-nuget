using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Instagram
{
    public class InstagramClient(HttpClient httpClient, IOptions<InstagramSettings> settings) : IInstagramClient
    {
        private readonly InstagramSettings _settings = settings.Value;
        
        public string GetAuthorizationUrl()
        {
            return InstagramEndpoints.GetAuthorizationUrl(settings.Value, InstagramScope.UserProfileCombinedScope);
        }

        #region Asynchronous

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>(InstagramConstants.ClientId, _settings.ClientId),
                new KeyValuePair<string, string>(InstagramConstants.ClientSecret, _settings.ClientSecret),
                new KeyValuePair<string, string>(InstagramConstants.GrantType, InstagramGrantType.AuthorizationCode),
                new KeyValuePair<string, string>(InstagramConstants.RedirectUri, _settings.RedirectUri),
                new KeyValuePair<string, string>(InstagramConstants.Code, code)
            });

            var response = await httpClient.PostAsync(InstagramEndpoints.GetAccessTokenUrl(), content);
            return await HandleResponseAsync(response);
        }

        public async Task<string> ExchangeForLongLivedTokenAsync(string shortLivedToken)
        {
            var url = InstagramEndpoints.GetLongLivedTokenUrl(_settings.ClientSecret, shortLivedToken);
            var response = await httpClient.GetAsync(url);
            return await HandleResponseAsync(response);
        }

        public async Task<string> RefreshLongLivedTokenAsync(string longLivedToken)
        {
            var url = InstagramEndpoints.GetRefreshTokenUrl(longLivedToken);
            var response = await httpClient.GetAsync(url);
            return await HandleResponseAsync(response);
        }

        public async Task<JsonElement> GetUserProfileAsync(string accessToken)
        {
            var url = InstagramEndpoints.GetUserProfileUrl(accessToken, InstagramFields.UserProfileFields);
            return await GetJsonElementAsync(url);
        }

        public async Task<JsonElement> GetUserMediaAsync(string accessToken)
        {
            var url = InstagramEndpoints.GetUserMediaUrl(accessToken, InstagramFields.UserMediaFields);
            return await GetJsonElementAsync(url);
        }

        public async Task<JsonElement> GetMediaAsync(string mediaId, string accessToken)
        {
            var url = InstagramEndpoints.GetMediaUrl(mediaId, accessToken, InstagramFields.MediaFields);
            return await GetJsonElementAsync(url);
        }

        public async Task<JsonElement> GetMediaChildrenAsync(string mediaId, string accessToken)
        {
            var url = InstagramEndpoints.GetMediaChildrenUrl(mediaId, accessToken, InstagramFields.MediaChildrenFields);
            return await GetJsonElementAsync(url);
        }

        public async Task<List<JsonElement>> GetAllUserMediaAsync(string accessToken)
        {
            var allMedia = new List<JsonElement>();
            var nextPageUrl = InstagramEndpoints.GetUserMediaUrl(accessToken, InstagramFields.UserMediaFields);

            while (!string.IsNullOrEmpty(nextPageUrl))
            {
                var response = await httpClient.GetAsync(nextPageUrl);
                response.EnsureSuccessStatusCode();

                var mediaData = await response.Content.ReadFromJsonAsync<JsonElement>();
                var data = mediaData.GetProperty(InstagramConstants.Data);
                allMedia.AddRange(data.EnumerateArray());

                nextPageUrl = mediaData.TryGetProperty(InstagramConstants.Paging, out JsonElement paging) &&
                              paging.TryGetPropertyAsString(InstagramConstants.Next, out var next)
                    ? next
                    : null;
            }

            return allMedia;
        }

        #endregion

        #region Helpers

        private async Task<JsonElement> GetJsonElementAsync(string? url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<JsonElement>();
        }

        private async Task<string> HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var tokenData = await response.Content.ReadFromJsonAsync<JsonElement>();
                return tokenData.GetPropertyAsString(InstagramConstants.AccessToken);
            }

            var errorData = await response.Content.ReadFromJsonAsync<JsonElement>();
            var errorMessage = errorData.GetNestedPropertyAsString(InstagramConstants.Error, InstagramConstants.Message);
            var errorCode = errorData.GetNestedPropertyAsInt(InstagramConstants.Error, InstagramConstants.Code);
            var errorSubCode = errorData.GetNestedPropertyAsInt(InstagramConstants.Error, InstagramConstants.ErrorSubCode);
            throw new InstagramApiException(errorMessage, errorCode, errorSubCode);
        }

        #endregion
    }
}