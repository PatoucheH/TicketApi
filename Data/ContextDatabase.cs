using Microsoft.EntityFrameworkCore;
using TicketApi.Models;

namespace TicketApi.Data
{
    public class ContextDatabase(DbContextOptions<ContextDatabase> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(u => u.Email)
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(t => t.Status)
                    .IsRequired()
                    .HasDefaultValue("open");
                //entity.Property(t => t.CreateAt)
                //    .IsRequired()
                //    .HasDefaultValue("CURRENT_TIMESTAMP");
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(t => t.UserId);
            });

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                modelBuilder.Entity<User>().HasData(
                    new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
                    new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
                );

            }
        }        
    }
}
