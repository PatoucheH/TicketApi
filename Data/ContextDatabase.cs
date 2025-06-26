using Microsoft.EntityFrameworkCore;
using TicketApi.Models;

namespace TicketApi.Data
{
    // class to create the context extends DbContext
    public class ContextDatabase(DbContextOptions<ContextDatabase> options) : DbContext(options)
    {
        // create the DbSet of Users
        public virtual DbSet<User> Users { get; set; }

        // create the DbSet of Tickets
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // create each column and parms of the table user
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.Email)
                    .HasMaxLength(150);
                entity.HasMany(u => u.Tickets)
                    .WithOne(t => t.User)
                    .HasForeignKey(t => t.UserId);
            });

            // create each column and parms of the table ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(t => t.Status)
                    .IsRequired()
                    .HasDefaultValue("open");
                entity.Property(t => t.CreateAt)
                    .IsRequired();
                    //.HasDefaultValue("CURRENT_TIMESTAMP");
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // if in development create some seed to fill the database
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
                    new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
                );

                modelBuilder.Entity<Ticket>().HasData(
                    new Ticket { Id=1, Title = "Ticket1", Status = "open", UserId = 2, CreateAt = new DateTime(2025, 01, 01, 10, 45,0) },
                    new Ticket { Id=2, Title = "Ticket2", Status = "open", UserId = 2, CreateAt = new DateTime(2025, 06, 25, 12, 0, 0) },
                    new Ticket { Id=3, Title = "Ticket3", Status = "open", UserId = 1, CreateAt = new DateTime(2025, 05, 04, 18, 0,0) },
                    new Ticket { Id=4, Title = "Ticket4", Status = "open", UserId = 2, CreateAt = new DateTime(2025, 02, 21, 08, 30,0) }
                );
            }
        }        
    }
}
