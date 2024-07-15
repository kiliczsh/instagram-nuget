namespace Instagram;

public class InstagramApiException(string message, int errorCode, int errorSubCode) : Exception(message)
{

    public int ErrorCode { get; } = errorCode;
    public int ErrorSubCode { get; } = errorSubCode;
}