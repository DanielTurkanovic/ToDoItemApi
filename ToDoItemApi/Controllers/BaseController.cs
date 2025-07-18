using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ToDoItemApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected bool TryGetUserId(out int userId)
        {
            userId = 0;
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdString, out userId);
        }
    }
}
