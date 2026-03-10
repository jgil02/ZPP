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
        public DbSet<Reservation> Reservations { get; set; }


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
