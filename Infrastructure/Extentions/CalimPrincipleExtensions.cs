using System.Security.Claims;

namespace Infrastructure.Extentions
{
    public static class CalimPrincipleExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
