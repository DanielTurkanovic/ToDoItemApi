using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoItemApi.ApplicationServices;
using ToDoItemApi.Data;
using ToDoItemApi.Models.Auth;
using BCrypt.Net;

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
                return Unauthorized("Invalid credentials");
            }

            var token = jwtTokenService.GenerateToken(user.Id.ToString(), user.Email);

            return Ok(new { Token = token });
        }
    }
}

