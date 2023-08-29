using Microsoft.AspNetCore.Identity;

namespace Core.Authentication
{
    public class HCBUser : IdentityUser
    {
        public string Nickname { get; set; } = string.Empty;
    }
}
