using Microsoft.AspNetCore.Identity;

namespace Core.Authentication
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Nickname { get; set; } = string.Empty;
    }
}
