using CarRentalApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarRentalApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }

        public DbSet<CarFleet> CarFleets { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Precyzja dla cen (decimal w SQL wymaga określenia skali)
            modelBuilder.Entity<Car>()
                .Property(c => c.PricePerDay)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Reservation>()
                .Property(r => r.TotalPrice)
                .HasPrecision(18, 2);

            // 2. Unikanie cykli kaskadowego usuwania (Cascade Delete)
            // Jeśli usuniesz klienta, nie chcemy, żeby SQL Server wywalił błąd 
            // z powodu wielu ścieżek dostępu do rezerwacji.

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict); // Zmienione z Cascade na Restrict

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.CarFleet)
                .WithMany(f => f.Reservations)
                .HasForeignKey(r => r.CarVin)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Twój Connection String do bazy danych
                optionsBuilder.UseSqlServer("Data Source=RAF-LT-PRO;Initial Catalog=Wypożyczalnia;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"); // Upewnij się, że ten Connection String jest poprawny i bezpieczny
            }
        }
    }
}
