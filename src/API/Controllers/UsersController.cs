using Core.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Authorize(Roles = HCBRoles.Admin)]
    public class UsersController : ApiControllerBase
    {
    }
}
