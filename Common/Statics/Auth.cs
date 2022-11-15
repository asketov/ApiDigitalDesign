namespace Common.Statics
{
    public static class Auth
    {
        public static int RefreshTokenLifeTime { get; } = 1200;
        public static int AccessTokenLifeTime { get; } = 104444444;
        public static string UserClaim { get; } = "userId";
        public static string RefreshClaim { get; } = "refreshTokenId";
        public static string SessionClaim { get; } = "sessionId";

    }
}
