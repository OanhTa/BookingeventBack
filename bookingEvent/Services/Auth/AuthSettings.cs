namespace bookingEvent.Services.Auth
{
    public class AuthSettings
    {
        public static string PrivateKey = "this-is-a-very-very-strong-secret-key-1234567890";

        // Thời gian sống của Access Token
        public static int AccessTokenExpiryMinutes = 15;

        // Thời gian sống của Refresh Token
        public static int RefreshTokenExpiryDays = 7;
    }
}
