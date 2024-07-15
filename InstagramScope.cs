namespace Instagram;

public static class InstagramScope
{
    private const string UserProfileScope = "user_profile";
    private const string UserMediaScope = "user_media";
    
    public static string UserProfileCombinedScope => string.Join(",", UserProfileScope, UserMediaScope);

}