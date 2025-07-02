using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TicketApi.Models;
using TicketApi.Services;

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
                entity.HasOne(t => t.User)
                    .WithMany(u => u.Tickets)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
