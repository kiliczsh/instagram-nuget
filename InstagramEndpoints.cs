namespace Instagram
{
    public static class InstagramEndpoints
    {
        private const string Graph = "https://graph.instagram.com";
        private const string Api = "https://api.instagram.com";

        public static string GetAuthorizationUrl(InstagramSettings settings, string scope, string responseType = "code")
        {
            return
                $"{Api}/oauth/authorize?client_id={settings.ClientId}&redirect_uri={settings.RedirectUri}&scope={scope}&response_type={responseType}";
        }

        public static string GetAccessTokenUrl()
        {
            return $"{Api}/oauth/access_token";
        }

        public static string GetLongLivedTokenUrl(string clientSecret, string shortLivedToken)
        {
            return
                $"{Graph}/access_token?grant_type=ig_exchange_token&client_secret={clientSecret}&access_token={shortLivedToken}";
        }

        public static string GetRefreshTokenUrl(string longLivedToken)
        {
            return $"{Graph}/refresh_access_token?grant_type=ig_refresh_token&access_token={longLivedToken}";
        }

        public static string GetUserProfileUrl(string accessToken, string fields)
        {
            return $"{Graph}/me?fields={fields}&access_token={accessToken}";
        }

        public static string GetUserMediaUrl(string accessToken, string fields)
        {
            return $"{Graph}/me/media?fields={fields}&access_token={accessToken}";
        }

        public static string GetMediaUrl(string mediaId, string accessToken, string fields)
        {
            return $"{Graph}/{mediaId}?fields={fields}&access_token={accessToken}";
        }

        public static string GetMediaChildrenUrl(string mediaId, string accessToken, string fields)
        {
            return $"{Graph}/{mediaId}/children?fields={fields}&access_token={accessToken}";
        }
    }
}