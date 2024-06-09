namespace Gintarine.ExternalClients.Post;

public static class Constants
{
    public const int MinAddressLength = 3;
    
    public static class Codes
    {
        public const int Success = 0;
        public const int TermParameterNotValid = 1001;
        public const int ReachDailyRequestLimit = 1002;
        public const int ApiKeyInvalid = 1003;
        public const int ApiKeyNotSet = 1004;
        public const int InvalidUrl = 404;
        public const int ServiceUnavailable = 503;
    }
    
    public static class Query
    {
        public const string Key = "key";
        public const string Term = "term";
    }
}