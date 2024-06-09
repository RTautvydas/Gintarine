namespace Gintarine.ExternalClients.Post;

public static class ErrorFactory
{
    public enum ErrorCode
    {
        InvalidUrl,
        ServiceUnavailable,
        ApiKeyInvalid,
        ReachDailyRequestLimit,
        ApiKeyNotSet,
        TermParameterNotValid,
        InvalidAddressLength,
        UnknownError
    }
    
    private static readonly Dictionary<ErrorCode, (string Message, bool IsCritical)> ErrorMessages = new()
    {
        { ErrorCode.InvalidUrl, ("Invalid URL.", true) },
        { ErrorCode.ServiceUnavailable, ("Service unavailable.", true) },
        { ErrorCode.ApiKeyInvalid, ("API key is not valid.", true) },
        { ErrorCode.ReachDailyRequestLimit, ("Reached daily request limit.", true) },
        { ErrorCode.ApiKeyNotSet, ("API key not set.", true) },
        { ErrorCode.TermParameterNotValid, ("Term parameter not valid.", false) },
        { ErrorCode.InvalidAddressLength, ("Invalid address length. Address should have at least " + Constants.MinAddressLength +" symbols.", false) },
        { ErrorCode.UnknownError, ("Unknown error occurred.", false) }
    };

    public static Error Create(ErrorCode errorCode)
    {
        if (!ErrorMessages.TryGetValue(errorCode, out var error))
        {
            error = ErrorMessages[ErrorCode.UnknownError];
        }

        return new Error(error.Message, error.IsCritical);
    }
}