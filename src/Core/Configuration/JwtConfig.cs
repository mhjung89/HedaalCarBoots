namespace Core.Configuration
{
    public class JwtConfig
    {
        public string ValidIssuer { get; set; } = null!;
        public string ValidAudience { get; set; } = null!;
        public string Secret { get; set; } = null!;
    }
}
