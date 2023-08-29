using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class HCBUser : IdentityUser
    {
        public string Nickname { get; set; } = string.Empty;
    }
}
