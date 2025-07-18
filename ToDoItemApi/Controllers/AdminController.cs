using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ToDoDbContext dbContext;

        public AdminController(ToDoDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/admin/deleted-users
        [HttpGet("deleted-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeletedUsers()
        {
            var deletedUsers = await dbContext.Users
                .IgnoreQueryFilters()
                .Where(u => u.IsDeleted)
                .ToListAsync();

            return Ok(deletedUsers);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("restore-user/{id:int}")]
        public async Task<IActionResult> RestoreUser(int id)
        {
            //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            //var currentUser = await dbContext.Users.FindAsync(userId);

            //if (currentUser == null || !currentUser.IsAdmin)
            //    return StatusCode(403, "Only admin can access this endpoint.");

            var deletedUser = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted);

            if (deletedUser == null)
                return NotFound("User not found or is not deleted.");

            deletedUser.IsDeleted = false;
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "User restored." });
        }
    }
}
