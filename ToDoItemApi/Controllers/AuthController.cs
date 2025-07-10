using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoItemApi.ApplicationServices;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Auth;
using ToDoItemApi.Models.Auth.ToDoItemApi.Models.Auth;
using ToDoItemApi.Models.Domain;

namespace ToDoItemApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenService jwtTokenService;
        private readonly ToDoDbContext dbContext;

        public AuthController(ToDoDbContext dbContext, IJwtTokenService jwtTokenService)
        {
            this.dbContext = dbContext;
            this.jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var token = jwtTokenService.GenerateToken(user.Id.ToString(), user.Email);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Email = user.Email
            });
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
    }
}
