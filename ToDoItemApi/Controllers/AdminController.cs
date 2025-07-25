using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoItemApi.Models.DTO;
using ToDoItemApi.Repositories;

namespace ToDoItemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public AdminController(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        // GET: api/admin/deleted-users
        [Authorize(Roles = "Admin")]
        [HttpGet("deleted-users")]
        public async Task<IActionResult> GetDeletedUsers()
        {
            var deletedUsers = await userRepository.GetDeletedUsersAsync();
            var deletedUsersDto = mapper.Map<List<UserDto>>(deletedUsers);

            return Ok(deletedUsersDto);
        }

        // DELETE: api/admin/delete-user/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-user/{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await userRepository.SoftDeleteUserAsync(id);

            if (!result)
                return BadRequest("User not found or already deleted.");

            return Ok(new { Message = "User has been soft-deleted." });
        }

        // PUT: api/admin/restore-user/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("restore-user/{id:int}")]
        public async Task<IActionResult> RestoreUser(int id)
        {
            var result = await userRepository.RestoreUserAsync(id);

            if (!result)
                return NotFound("User not found or is not deleted.");

            return Ok(new { Message = "User restored." });
        }
    }
}
