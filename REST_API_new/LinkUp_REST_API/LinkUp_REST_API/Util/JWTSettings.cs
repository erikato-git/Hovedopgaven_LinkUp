namespace LinkUp_REST_API.Util
{
    public class JwtSettings
    {
        public required string Secret { get; set; }
        public required string Issuers { get; set; }
        public required string Audience { get; set; }
        public int ExpirationTimeInMinutes { get; set; }
        public required string Issuer { get; set; }
    }
}
