using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Domain;
using ToDoItemApi.Models.DTO;

namespace ToDoItemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ToDoDbContext dbContext;
        private readonly IMapper mapper;

        public AdminController(ToDoDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
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

            var deletedUsersDto = mapper.Map<List<UserDto>>(deletedUsers);

            return Ok(deletedUsersDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound("User not found.");

            if (user.IsDeleted)
                return BadRequest("User is already deleted.");

            user.IsDeleted = true;
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "User has been soft-deleted." });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("restore-user/{id:int}")]
        public async Task<IActionResult> RestoreUser(int id)
        {
            var deletedUser = await dbContext.Users
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == id && u.IsDeleted);

            if (deletedUser == null)
                return NotFound("User not found or is not deleted.");

            deletedUser.IsDeleted = false;
            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "User restored." });
        }
    }
}
