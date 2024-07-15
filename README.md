# Instagram Library

The `Instagram` library provides a simple and easy-to-use client for interacting with the Instagram API. This library helps you handle common tasks such as obtaining access tokens, exchanging short-lived tokens for long-lived ones, refreshing tokens, and retrieving user profiles and media.

## Installation

Install the package via NuGet:

```bash
dotnet add package Instagram
```

## Configuration

Add your Instagram settings to the appsettings.json file:

```json
{
  "InstagramSettings": {
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret",
    "RedirectUri": "https://localhost:8081/api/instagram/callback"
  }
}
```

## ASP.NET Core Setup

In your Program.cs or Startup.cs file, configure the InstagramClient and its dependencies:

```csharp
// Instagram Client Configuration
builder.Services.Configure<InstagramSettings>(builder.Configuration.GetSection("InstagramSettings"));
builder.Services.AddHttpClient<IInstagramClient, InstagramClient>();
```

Usage

You can now inject and use the InstagramClient in your controllers or services. Here’s an example of how to use it in a controller:
    
```csharp
public class InstagramController : Controller
{
    private readonly IInstagramClient _instagramClient;

    public InstagramController(IInstagramClient instagramClient)
    {
        _instagramClient = instagramClient;
    }

    public async Task<IActionResult> SignIn()
    {
        var url = _instagramClient.GetAuthorizationUrl();
        return Redirect(url);
    }

    public async Task<IActionResult> Callback(string code)
    {
        var token = await _instagramClient.GetAccessToken(code);
        var user = await _instagramClient.GetUserProfile(token.AccessToken);
        var media = await _instagramClient.GetUserMedia(token.AccessToken, user.Id);
        return Ok(media);
    }
}
```

## Methods

The InstagramClient class provides the following methods:

```csharp
string GetAuthorizationUrl()
Task<string> GetAccessTokenAsync(string code)
Task<string> ExchangeForLongLivedTokenAsync(string shortLivedToken)
Task<string> RefreshLongLivedTokenAsync(string longLivedToken)
Task<JsonElement> GetUserProfileAsync(string accessToken)
Task<JsonElement> GetUserMediaAsync(string accessToken)
Task<JsonElement> GetMediaAsync(string mediaId, string accessToken)
Task<JsonElement> GetMediaChildrenAsync(string mediaId, string accessToken)
Task<List<JsonElement>> GetAllUserMediaAsync(string accessToken)
```

## License

See the LICENSE file for more details.

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.
