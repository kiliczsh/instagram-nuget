namespace Instagram
{
    public static class InstagramFields
    {
        private const string Id = "id";
        private const string Username = "username";
        private const string AccountType = "account_type";
        private const string MediaCount = "media_count";
        private const string Caption = "caption";
        private const string MediaType = "media_type";
        private const string MediaUrl = "media_url";
        private const string ThumbnailUrl = "thumbnail_url";
        private const string Permalink = "permalink";
        private const string Timestamp = "timestamp";

        public static string UserProfileFields => string.Join(",", Id, Username, AccountType, MediaCount);
        public static string UserMediaFields => string.Join(",", Id, Caption, MediaType, MediaUrl, ThumbnailUrl, Permalink, Timestamp);
        public static string MediaFields => string.Join(",", Id, Caption, MediaType, MediaUrl, ThumbnailUrl, Permalink, Timestamp);
        public static string MediaChildrenFields => string.Join(",", Id, MediaType, MediaUrl, ThumbnailUrl);
    }
}