using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketApi.Data;
using TicketApi.Models;
using TicketApi.Models.DTOs;
using TicketApi.Services;

namespace TicketApi.Controllers
{
    
    [ApiController]
    // Base Route for this Controller
    [Route("api/[controller]")]

    // All the controller for Ticket
    public class TicketController(ContextDatabase context, ILogger<UserController> logger) : ControllerBase
    {
        // create context the logger and the TicketService
        private readonly ContextDatabase _context = context;
        private readonly ILogger<UserController> _logger = logger;
        private TicketService _ticketService = new TicketService(context);

        // Get all the tickets
        [HttpGet]    
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTickets()
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ticketService.GetTickets();
                var ticketsDto = tickets.Select(t => new TicketDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    CreateAt = t.CreateAt,
                    //UserId = t.UserId
                });
                return Ok(ticketsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTickets");
                return StatusCode(500, $"An error occured when retrieving tickets {ex.Message}");
            }
        }

        // Get the tickets with an userId choosen
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByUserId(int userId)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ticketService.GetTicketsByUserId(userId);
                var ticketsDto = tickets.Select(t => new TicketDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    CreateAt = t.CreateAt,
                    //UserId = t.UserId
                });
                return Ok(ticketsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTicketsByUserId");
                return StatusCode(500, $"An error occured when retrieving tickets {ex.Message}");
            }
        }


        // get all the ticket with an special status like open 
        [HttpGet("{status}")]
        public async Task<ActionResult<IEnumerable<TicketDTO>>> GetTicketsByStatus(string status)
        {
            try
            {
                IEnumerable<Ticket> tickets = await _ticketService.GetTicketsByStatus(status);
                var ticketsDto = tickets.Select(t => new TicketDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status,
                    CreateAt = t.CreateAt,
                    //UserId = t.UserId
                });
                return Ok(ticketsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetTicketsByStatus");
                return StatusCode(500, $"An error occured while retrieving tickets {ex.Message}");
            }
        }

        //Get a ticket by its id 

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Ticket>> GetTicketById(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket is null) return NotFound();
            var ticketDto = new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                CreateAt = ticket.CreateAt,
                //UserId = ticket.UserId
            };
            return Ok(ticketDto);
        }

        // Create a new ticket
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public async Task<ActionResult<TicketDTO>> CreateTicket([FromBody] TicketCreateDTO ticketDto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim is null) return Unauthorized("User id not found in token");
                var userId = int.Parse(userIdClaim.Value);

                bool userExists = _context.Users.Any(u => u.Id == userId);
                if (!userExists) return BadRequest("The Id of the user selected doesn't exists ! ");

                var ticket = new Ticket
                {
                    Title = ticketDto.Title,
                    Status = "open",
                    CreateAt = DateTime.UtcNow,
                    UserId = userId
                };
                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                var createdTicket = new TicketDTO
                {
                    Id = ticket.Id,
                    Title = ticket.Title,
                    Status = ticket.Status,
                    CreateAt = ticket.CreateAt,
                    //UserId = ticket.UserId
                };
                return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, createdTicket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error CreateTicket");
                return StatusCode(500, $"An error occured while creating the ticket {ex.Message}");
            }
        }

        //Delete one ticket by its id 
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTicket(int id )
        {
            try
            {
                await _ticketService.DeleteTicket(id);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting ticket with id {id}");
                return StatusCode(500, "An error occured while deleting the ticket.");
            }
        }

        // Update title ticket by its id
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("title/{id}")]
        public async Task<ActionResult> UpdateTitleTicket(int id, string newTitle)
        {
            try
            {
                await _ticketService.UpdateTitleTicket(id, newTitle);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while updating the title of the ticket with id {id}");
                return StatusCode(500, "An error occured while updating the ticket");
            }
        }

        // Update Status ticket by its id 
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("status/{id}")]
        public async Task<ActionResult> UpdateStatusTicket(int id, string newStatus)
        {
            try
            {
                await _ticketService.UpdateStatusTicket(id, newStatus);
                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error while updating the status of the ticket with id {id}");
                return StatusCode(500, "An error occured while updating the ticket");
            }
        }
    }
}
