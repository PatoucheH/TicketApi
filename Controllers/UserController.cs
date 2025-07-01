using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketApi.Data;
using TicketApi.Models;
using TicketApi.Models.DTOs;
using TicketApi.Services;

namespace TicketApi.Controllers
{
    
    [ApiController] 
    // Basz route for this controlelr
    [Route("api/[controller]")]
    public class UserController(ContextDatabase context, ILogger<UserController> logger) : ControllerBase
    {
        // create the context, the logegr and the UserService
        private readonly ContextDatabase _context = context;
        private readonly ILogger<UserController> _logger = logger;
        private UserService _userService = new UserService(context);


        // get all the User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            try
            {
                IEnumerable<User> users = await _userService.GetUsers();
                var usersDto = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email
                });
                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUsers");
                return StatusCode(500, $"An error occured while retrieving user {ex.Message}");
            }
        }

        // get one user by his ID
        [HttpGet("/{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            try
            {
                User? user = await _userService.GetUserById(id);
                if (user is null) return NotFound($"No user with id {id}");
                var userDto = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
                return Ok(userDto);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserById");
                return StatusCode(500, "An error occured while retrieving user ");
            }
        }

        // Create a new user
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser([FromBody] UserDTO userDto)
        {
            try
            {
                var user = new User
                {
                    Name = userDto.Name,
                    Email = userDto.Email  
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var createdUser = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, createdUser);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error CreateUser");
                return StatusCode(500, $"An error occured while creating the user {ex.Message}");
            }
        }

        // Delete a user by its id 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteUser(int id )
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error DeleteUser");
                return StatusCode(500, "An error occured while deleting the user ");
            }
        }

        // Update all the info about one user by its ID 
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult> UpdateAllInfoAboutOneUSer(int id, UserDTO userDto)
        {
            try
            {
                if (id != userDto.Id) return BadRequest("None user with this Id");
                var user = new User{
                    Id = userDto.Id,
                    Name = userDto.Name,
                    Email = userDto.Email
                };
                await _userService.UpdateAllInfoAboutOneUser(id, user);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error UpdateAllInfoAboutOneUser");
                return StatusCode(500, "An error occured while updating the user");
            }
        }
    } 
}
