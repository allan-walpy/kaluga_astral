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

            this.ConfigureInhabitants(ref modelBuilder);
            this.ConfigureCustomers(ref modelBuilder);
            this.ConfigureRooms(ref modelBuilder);
        }

        private void ConfigureCustomers(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => new
                {
                    c.FirstName,
                    c.SecondName,
                    c.ThirdName
                }).IsUnique();

            modelBuilder.Entity<Customer>()
                .Property(c => c.ThirdName)
                .HasDefaultValue(null);
        }

        private void ConfigureRooms(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                 .HasIndex(r => r.Number).IsUnique();

            modelBuilder.Entity<Room>()
                .Property(r => r.Capacity)
                .HasDefaultValue(1);

            modelBuilder.Entity<Room>()
                .Property(r => r.Category)
                .HasDefaultValue(RoomCategory.Standart);
        }

        private void ConfigureInhabitants(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inhabitant>()
                .HasOne(i => i.Room)
                .WithOne(r => r.Inhabitant)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey<Room>(r => r.InhabitantId)
                .HasPrincipalKey<Inhabitant>(i => i.RoomId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Inhabitant>()
                .HasOne(i => i.Customer)
                .WithOne(c => c.Inhabitant)
                .OnDelete(DeleteBehavior.SetNull)
                .HasForeignKey<Customer>(c => c.InhabitantId)
                .HasPrincipalKey<Inhabitant>(i => i.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        }


    }
}