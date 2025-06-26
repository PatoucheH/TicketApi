using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using TicketApi.Data;
using TicketApi.Models;


namespace TicketApi.Services
{
    /// <summary>
    /// Create the Interface for ticket
    /// </summary>
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<IEnumerable<Ticket>> GetTicketsByUserId(int userId);
        Task<Ticket?> GetTicketById(int id);
        Task<Ticket> CreateTickets(Ticket ticket);
        Task DeleteTicket(int id);
        Task UpdateTitleTicket(int id, string newTitle);
        Task UpdateStatusTicket(int id, string newTitle);
    }
    /// <summary>
    /// Create the class for service extends the interface above
    /// </summary>
    /// <param name="context">receive the context which extends DbContext</param>
    public class TicketService(ContextDatabase context) : ITicketService
    {
        private readonly ContextDatabase _context = context;

        /// <summary>
        /// Get all the tickets
        /// </summary>
        /// <returns>List of all the tickets</returns>
        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _context.Tickets.Include(t => t.User).ToListAsync();
        }

        /// <summary>
        /// Retrieves all tickets associated with the specified user ID.    
        /// </summary>
        /// <param name="id">The unique identifier of the user whose tickets are to be retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <IEnumerable{T}> of <Ticket>
        /// objects associated with the specified user ID. If no tickets are found, an empty collection is returned.</returns>
        public async Task<IEnumerable<Ticket>> GetTicketsByUserId(int id )
        {
            return await _context.Tickets.Where(t => t.UserId == id).ToListAsync();
        }

        /// <summary>
        /// Retrieves a collection of tickets that match the specified status.
        /// </summary>
        /// <param name="status">The status of the tickets to retrieve. This value is case-sensitive and must not be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an <see cref="IEnumerable{T}"/>
        /// of <see cref="Ticket"/> objects  where each ticket has the specified status. If no tickets match the status,
        /// an empty collection is returned.</returns>
        public async Task<IEnumerable<Ticket>> GetTicketsByStatus(string status)
        {
            return await _context.Tickets.Where(t => t.Status == status).ToListAsync();
        }

        public async Task<Ticket?> GetTicketById(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        /// <summary>
        /// Creates a new ticket and saves it to the database.
        /// </summary>
        /// <param name="ticket">The ticket to be created. Must not be null.</param>
        /// <returns>The created <see cref="Ticket"/> instance, including any database-generated values.</returns>
        public async Task<Ticket> CreateTickets(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
        /// <summary>
        /// Deletes a ticket with the specified identifier from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to delete.</param>
        /// <returns></returns>
        public async Task DeleteTicket(int id)
        {
            var ticket = new Ticket{ Id = id };

            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the title of an existing ticket in the database.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to update.</param>
        /// <param name="newTitle">The new title to assign to the ticket. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateTitleTicket(int id, string newTitle)
        {
            var ticket = new Ticket { Id = id, Title = newTitle };
            _context.Attach(ticket);
            _context.Entry(ticket).Property(t => t.Title).IsModified = true;
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Updates the status of a ticket with the specified identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket to update.</param>
        /// <param name="newStatus">The new status to assign to the ticket. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task UpdateStatusTicket(int id, string newStatus)
        {
            var ticket = new Ticket { Id = id, Status = newStatus };
            _context.Attach(ticket);
            _context.Entry(ticket).Property(t => t.Status).IsModified = true;
            await _context.SaveChangesAsync();
        }
     }
}
