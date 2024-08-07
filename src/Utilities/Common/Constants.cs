namespace TucaAPI.src.Common
{
    public static class Constants
    {
        public const int DEFAULT_PAGE_LIMIT = 25;
        public const int MIN_PASSWORD_LENGTH = 8;
        public const string DEFAULT_JWT_SECRET = "secret";
        public const int JWT_DAYS_TO_EXPIRES = 7;
        public const int MAX_LOGIN_ATTEMPTS = 10;
        public static TimeSpan TOKEN_EXPIRES_IN = TimeSpan.FromHours(1);
        public static TimeSpan RESET_LOGIN_ATTEMPTS = TimeSpan.FromHours(1);
        public static TimeSpan GOOGLE_AUTHENTICATOR_TIME_TOLERANCE = TimeSpan.FromSeconds(30);
        public static TimeSpan TIME_TO_ENABLE_PASSWORD_USAGE = TimeSpan.FromDays(-(5 * 30));
        public static TimeSpan TIME_TO_NOTIFY_THAT_PASSWORD_HAS_BEEN_CHANGED = TimeSpan.FromDays(
            -30
        );
    }
}
