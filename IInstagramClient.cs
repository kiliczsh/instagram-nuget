using System.Text.Json;

namespace Instagram;

public interface IInstagramClient
{
    string GetAuthorizationUrl();
    Task<string> GetAccessTokenAsync(string code);
    Task<string> ExchangeForLongLivedTokenAsync(string shortLivedToken);
    Task<string> RefreshLongLivedTokenAsync(string longLivedToken);
    Task<JsonElement> GetUserProfileAsync(string accessToken);
    Task<JsonElement> GetUserMediaAsync(string accessToken);
    Task<JsonElement> GetMediaAsync(string mediaId, string accessToken);
    Task<JsonElement> GetMediaChildrenAsync(string mediaId, string accessToken);
    Task<List<JsonElement>> GetAllUserMediaAsync(string accessToken);
}