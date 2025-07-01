using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TicketApi.Models;

namespace TicketApi.Data
{
    public class DbSeeder
    {
        /// <summary>
        /// Seeds the database with initial data if it is empty.
        /// </summary>
        /// <param name="context">The database context to seed.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task Seed(ContextDatabase context)
        {
            if (!context.Users.Any())
            {
                var hasher = new PasswordHasher<User>();

                var admin = new User
                {
                    Name = "Hugo",
                    Email = "Hugo@example.com",
                    Role = "Admin",
                };
                admin.PasswordHash = hasher.HashPassword(admin, "Admin");

                var user = new User
                {
                    Name = "Bob",
                    Email = "bob@example.com",
                    Role = "User",
                };
                user.PasswordHash = hasher.HashPassword(user, "Bob123");

                context.Users.AddRange(admin, user);
                await context.SaveChangesAsync();
            }

            if (!context.Tickets.Any())
            {
                var tickets = new List<Ticket>
                {
                    new Ticket { Title = "Ticket1", Status = "open", UserId = 2, CreateAt = new DateTime(2025, 01, 01, 10, 45,0) },
                    new Ticket { Title = "Ticket2", Status = "open", UserId = 2, CreateAt = new DateTime(2025, 06, 25, 12, 0, 0) },
                    new Ticket { Title = "Ticket3", Status = "open", UserId = 1, CreateAt = new DateTime(2025, 05, 04, 18, 0,0) },
                    new Ticket { Title = "Ticket4", Status = "open", UserId = 2, CreateAt = new DateTime(2025, 02, 21, 08, 30,0) }
                };

                context.Tickets.AddRange(tickets);
                await context.SaveChangesAsync();
            }
        }
    }
}
