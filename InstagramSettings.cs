namespace Instagram;

public class InstagramSettings(string clientId, string clientSecret, string redirectUri)
{
    public string ClientId { get; set; } = clientId;
    public string ClientSecret { get; set; } = clientSecret;
    public string RedirectUri { get; set; } = redirectUri;
}