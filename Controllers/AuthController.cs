using Microsoft.AspNetCore.Mvc;
using TicketApi.Models;
using TicketApi.Models.Auth;
using TicketApi.Services;

namespace TicketApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService userService, IAuthService authService) : ControllerBase
    {
        // Controller to register one user
        [HttpPost("register")]
        public async Task<IActionResult> Register(LoginRequest request)
        {
            var existingUser = await userService.GetUsers();
            if (existingUser.Any(u => u.Email == request.Email))
                return BadRequest("User already exists");

            var user = new User
            {
                Email = request.Email,
                Name = request.Email.Split('@')[0], 
                PasswordHash = authService.HashPassword(request.Password)
            };

            await userService.CreateUser(user);
            return Ok("User registered");
        }

        // to login as an user
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var users = await userService.GetUsers();
            var user = users.FirstOrDefault(u => u.Email == request.Email);
            if (user is null) return Unauthorized("Invalid credentials");

            if (!authService.VerifyPassword(user, request.Password))
                return Unauthorized("Invalid credentials!!!");

            var token = authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}
