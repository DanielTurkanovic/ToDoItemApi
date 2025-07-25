using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToDoItemApi.ApplicationServices;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Auth;
using ToDoItemApi.Models.Auth.ToDoItemApi.Models.Auth;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly ToDoDbContext dbContext;
        private readonly IJwtTokenService jwtTokenService;

        public AuthController(ToDoDbContext dbContext, IJwtTokenService jwtTokenService)
        {
            this.dbContext = dbContext;
            this.jwtTokenService = jwtTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { Message = "Email and password are required." });

            if (await dbContext.Users.AnyAsync(x => x.Email == request.Email))
                return Conflict(new { Message = "User with this email already exists." });

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return Ok(new RegisterResponseDto { Message = "User successfully registered." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var role = user.IsAdmin ? "Admin" : "User";
            var token = jwtTokenService.GenerateToken(user.Id.ToString(), user.Email, role);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Email = user.Email
            });
        }

        [HttpDelete("delete-account")]
        [Authorize]
        public async Task<IActionResult> DeleteAccount()
        {
            if (!TryGetUserId(out int userId))
                return Unauthorized(new { Message = "Invalid user ID." });

            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound(new { Message = "User not found." });

            if (user.IsAdmin)
                return BadRequest(new { Message = "Admin account cannot be deleted." });

            user.IsDeleted = true;

            var userItems = await dbContext.ToDoItems
                .Where(t => t.UserId == userId)
                .ToListAsync();

            foreach (var item in userItems)
            {
                item.IsDeleted = true;
            }

            await dbContext.SaveChangesAsync();

            return Ok(new { Message = "User account deleted successfully." });
        }
    }
}
