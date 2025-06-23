using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using TicketApi.Models;

namespace TicketApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "/openapi/{documentName}.json";
                });
                app.MapScalarApiReference();
            }

            List<User> users = new();
            List<Ticket> tickets = new();

            var ticketIdCounter = 1;
            var userIdCounter = 1;

            // GET

            // Get all users
            app.MapGet("/users", () => users);

            // Get one users
            app.MapGet("/users/{id}", (int id) =>
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                return user is not null ? Results.Ok(user) : Results.NotFound($"No user with the id : {id}");
            });

            // get all tickets of an user
            app.MapGet("/users/{id}/tickets", (int id) =>
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                var ticketList = tickets.Where(t => t.Id == user.Id);
                return user is not null ? Results.Ok(ticketList) : Results.NotFound($"User {id} not found");
            });

            // Get one ticket from his Id
            app.MapGet("/tickets/{id}", (int id) =>
            {
                var ticket = tickets.FirstOrDefault(t => t.Id == id);
                return ticket is not null ? Results.Ok(ticket) : Results.NotFound($"Ticket with ID {id} not found");
            });

            // Get tickets with status 
            app.MapGet("/tickets/status/{status}", (string? status) =>
            {
                var result = string.IsNullOrEmpty(status) ? tickets : tickets.Where(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
                return Results.Ok(result);
            });

            // POST

            // Post one user
            app.MapPost("/users", (User user) =>
            {
                user.Id = userIdCounter++;
                users.Add(user);
                return Results.Created($"/users/{user.Id}", user);
            });

            // Post one ticket to a user 
            app.MapPost("/tickets", (Ticket ticket) =>
            {
                ticket.Id = ticketIdCounter++;
                ticket.Status = "open";
                ticket.SetCreateAtNow();
                tickets.Add(ticket);
                return Results.Created($"/tickets/{ticket.Id}", ticket);
            });

            // PUT

            // Put (modify) one user by id 
            app.MapPut("/users/{id}", (int id, User updateUser) =>
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user is null) return Results.NotFound();
                user.Name = updateUser.Name;
                user.Email = updateUser.Email;
                return Results.Ok(user);
            });

            // Put (modify) one ticket by its id 
            app.MapPut("/tickets/{id}", (int id, Ticket ticketToChange) =>
            {
                var ticket = tickets.FirstOrDefault(t => t.Id == id);
                if (ticket is null) return Results.NotFound($"Ticket with ID {id} not found");
                ticket.Title = ticketToChange.Title;
                ticket.Status = ticketToChange.Status;
                return Results.Ok(ticket);

            });

            // DELETE

            // Delete one user by Id
            app.MapDelete("/users/{id}", (int id) =>
            {
                var user = users.FirstOrDefault(u => u.Id == id);
                if (user is null) return Results.NotFound();
                users.Remove(user);
                return Results.NoContent();
            });

            // Delete one ticket by Id
            app.MapDelete("/tickets/{id}", (int id) =>
            {
                var ticket = tickets.FirstOrDefault(t => t.Id == id);
                if (ticket is null) return Results.NotFound($"Ticket with ID {id} not found");
                var user = users.FirstOrDefault(u => u.Id == ticket.UserId);
                if (user is null) return Results.NotFound($"User with ID {ticket.UserId} not found");

                tickets.Remove(ticket);
                return Results.NoContent();
            });



            app.Run();
        }
    }
}
