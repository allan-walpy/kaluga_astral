using System;
using Microsoft.EntityFrameworkCore;

namespace Hostel.Server.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Inhabitant> Inhabitants { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Identity> Identity { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasIndex(c => new
                {
                    c.FirstName,
                    c.SecondName,
                    c.ThirdName
                }).IsUnique();

            modelBuilder.Entity<Inhabitant>()
                .HasOne(i => i.Room)
                .WithOne(r => r.Inhabitant)
                .HasForeignKey<Room>(r => r.InhabitantId);

            modelBuilder.Entity<Inhabitant>()
                .HasOne(i => i.Customer)
                .WithOne(c => c.Inhabitant)
                .HasForeignKey<Customer>(c => c.InhabitantId);
        }
    }
}