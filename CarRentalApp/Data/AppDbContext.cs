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


            modelBuilder.Entity<Car>().HasData(
                new Car { IdCar = "C1", Brand = "Toyota", Model = "Corolla", PricePerDay = 150, ImagePath = "corolla.jpg", GearboxType = "Manual", FuelType = "Hybrid", BodyType = "Sedan", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 470, EngineCapacity = 1.8 },
                new Car { IdCar = "C2", Brand = "BMW", Model = "3 Series", PricePerDay = 300, ImagePath = "bmw3.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Sedan", SeatsCount = 5, Segment = "D", DoorsCount = 4, TrunkCapacity = 480, EngineCapacity = 2.0 },
                new Car { IdCar = "C3", Brand = "Tesla", Model = "Model 3", PricePerDay = 450, ImagePath = "tesla3.jpg", GearboxType = "Automatic", FuelType = "Electric", BodyType = "Sedan", SeatsCount = 5, Segment = "C", DoorsCount = 4, TrunkCapacity = 542, EngineCapacity = 0.0 },

                // SKODA
                new Car { IdCar = "C4", Brand = "Skoda", Model = "Octavia 3", PricePerDay = 180, ImagePath = "octavia3.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Liftback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 610, EngineCapacity = 1.4 },
                new Car { IdCar = "C5", Brand = "Skoda", Model = "Octavia 4", PricePerDay = 220, ImagePath = "octavia4.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Combi", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 600, EngineCapacity = 2.0 },
                new Car { IdCar = "C6", Brand = "Skoda", Model = "Fabia 3", PricePerDay = 120, ImagePath = "fabia3.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 330, EngineCapacity = 1.0 },
                new Car { IdCar = "C7", Brand = "Skoda", Model = "Fabia 4", PricePerDay = 140, ImagePath = "fabia4.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 380, EngineCapacity = 1.0 },
                new Car { IdCar = "C8", Brand = "Skoda", Model = "Superb 3", PricePerDay = 280, ImagePath = "superb3.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Sedan", SeatsCount = 5, Segment = "D", DoorsCount = 5, TrunkCapacity = 625, EngineCapacity = 2.0 },
                new Car { IdCar = "C9", Brand = "Skoda", Model = "Superb 4", PricePerDay = 350, ImagePath = "superb4.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Combi", SeatsCount = 5, Segment = "D", DoorsCount = 5, TrunkCapacity = 690, EngineCapacity = 2.0 },

                // VOLKSWAGEN
                new Car { IdCar = "C10", Brand = "Volkswagen", Model = "Polo 5", PricePerDay = 110, ImagePath = "polo5.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 280, EngineCapacity = 1.2 },
                new Car { IdCar = "C11", Brand = "Volkswagen", Model = "Polo 6", PricePerDay = 135, ImagePath = "polo6.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 351, EngineCapacity = 1.0 },
                new Car { IdCar = "C12", Brand = "Volkswagen", Model = "Golf 6", PricePerDay = 150, ImagePath = "golf6.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 350, EngineCapacity = 1.4 },
                new Car { IdCar = "C13", Brand = "Volkswagen", Model = "Golf 7", PricePerDay = 170, ImagePath = "golf7.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 380, EngineCapacity = 1.6 },
                new Car { IdCar = "C14", Brand = "Volkswagen", Model = "Golf 8", PricePerDay = 210, ImagePath = "golf8.jpg", GearboxType = "Automatic", FuelType = "Hybrid", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 381, EngineCapacity = 1.5 },
                new Car { IdCar = "C15", Brand = "Volkswagen", Model = "Passat 7", PricePerDay = 190, ImagePath = "passat7.jpg", GearboxType = "Manual", FuelType = "Diesel", BodyType = "Sedan", SeatsCount = 5, Segment = "D", DoorsCount = 4, TrunkCapacity = 565, EngineCapacity = 2.0 },
                new Car { IdCar = "C16", Brand = "Volkswagen", Model = "Passat 8", PricePerDay = 230, ImagePath = "passat8.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Combi", SeatsCount = 5, Segment = "D", DoorsCount = 5, TrunkCapacity = 650, EngineCapacity = 2.0 },
                new Car { IdCar = "C17", Brand = "Volkswagen", Model = "Passat 9", PricePerDay = 320, ImagePath = "passat9.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Combi", SeatsCount = 5, Segment = "D", DoorsCount = 5, TrunkCapacity = 690, EngineCapacity = 2.0 },

                // SEAT
                new Car { IdCar = "C18", Brand = "Seat", Model = "Leon 3", PricePerDay = 160, ImagePath = "leon3.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 380, EngineCapacity = 1.4 },
                new Car { IdCar = "C19", Brand = "Seat", Model = "Leon 4", PricePerDay = 200, ImagePath = "leon4.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 380, EngineCapacity = 1.5 },
                new Car { IdCar = "C20", Brand = "Seat", Model = "Ibiza 4", PricePerDay = 110, ImagePath = "ibiza4.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 292, EngineCapacity = 1.2 },
                new Car { IdCar = "C21", Brand = "Seat", Model = "Ibiza 5", PricePerDay = 130, ImagePath = "ibiza5.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 355, EngineCapacity = 1.0 },

                // AUDI
                new Car { IdCar = "C22", Brand = "Audi", Model = "A3", PricePerDay = 250, ImagePath = "audia3.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 380, EngineCapacity = 1.5 },
                new Car { IdCar = "C23", Brand = "Audi", Model = "S3", PricePerDay = 500, ImagePath = "audis3.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 325, EngineCapacity = 2.0 },
                new Car { IdCar = "C24", Brand = "Audi", Model = "RS3", PricePerDay = 850, ImagePath = "audirs3.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "C", DoorsCount = 5, TrunkCapacity = 282, EngineCapacity = 2.5 },
                new Car { IdCar = "C25", Brand = "Audi", Model = "A4", PricePerDay = 300, ImagePath = "audia4.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Sedan", SeatsCount = 5, Segment = "D", DoorsCount = 4, TrunkCapacity = 460, EngineCapacity = 2.0 },
                new Car { IdCar = "C26", Brand = "Audi", Model = "S4", PricePerDay = 600, ImagePath = "audis4.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "Sedan", SeatsCount = 5, Segment = "D", DoorsCount = 4, TrunkCapacity = 420, EngineCapacity = 3.0 },
                new Car { IdCar = "C27", Brand = "Audi", Model = "RS4", PricePerDay = 1000, ImagePath = "audirs4.jpg", GearboxType = "Automatic", FuelType = "Petrol", BodyType = "Combi", SeatsCount = 5, Segment = "D", DoorsCount = 5, TrunkCapacity = 495, EngineCapacity = 2.9 },

                // POZOSTAŁE
                new Car { IdCar = "C28", Brand = "Opel", Model = "Corsa", PricePerDay = 115, ImagePath = "corsa.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 309, EngineCapacity = 1.2 },
                new Car { IdCar = "C29", Brand = "Kia", Model = "Sportage", PricePerDay = 240, ImagePath = "sportage.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "SUV", SeatsCount = 5, Segment = "SUV", DoorsCount = 5, TrunkCapacity = 591, EngineCapacity = 1.6 },
                new Car { IdCar = "C30", Brand = "Kia", Model = "Stonic", PricePerDay = 160, ImagePath = "stonic.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Crossover", SeatsCount = 5, Segment = "B-SUV", DoorsCount = 5, TrunkCapacity = 332, EngineCapacity = 1.2 },
                new Car { IdCar = "C31", Brand = "Volvo", Model = "XC40", PricePerDay = 350, ImagePath = "xc40.jpg", GearboxType = "Automatic", FuelType = "Hybrid", BodyType = "SUV", SeatsCount = 5, Segment = "SUV", DoorsCount = 5, TrunkCapacity = 452, EngineCapacity = 2.0 },
                new Car { IdCar = "C32", Brand = "Volvo", Model = "XC60", PricePerDay = 450, ImagePath = "xc60.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "SUV", SeatsCount = 5, Segment = "SUV", DoorsCount = 5, TrunkCapacity = 483, EngineCapacity = 2.0 },
                new Car { IdCar = "C33", Brand = "Volvo", Model = "XC90", PricePerDay = 650, ImagePath = "xc90.jpg", GearboxType = "Automatic", FuelType = "Diesel", BodyType = "SUV", SeatsCount = 7, Segment = "SUV", DoorsCount = 5, TrunkCapacity = 640, EngineCapacity = 2.0 },
                new Car { IdCar = "C34", Brand = "Renault", Model = "Clio", PricePerDay = 120, ImagePath = "clio.jpg", GearboxType = "Manual", FuelType = "Petrol", BodyType = "Hatchback", SeatsCount = 5, Segment = "B", DoorsCount = 5, TrunkCapacity = 391, EngineCapacity = 1.0 }
            );

            modelBuilder.Entity<CarFleet>().HasData(
                new CarFleet { Vin = "VIN12345678901234", RegistrationNumber = "KR12345", CarId = "C1", IsAvailable = true, Mileage = 15000 },
                new CarFleet { Vin = "VIN98765432109876", RegistrationNumber = "WA54321", CarId = "C2", IsAvailable = true, Mileage = 5000 },
                new CarFleet { Vin = "VIN00000000000000", RegistrationNumber = "PO99999", CarId = "C3", IsAvailable = false, Mileage = 1200 },
                // 3x Skoda Octavia 3 (ID: C4)
                new CarFleet { Vin = "SKO3OCT0000000001", RegistrationNumber = "S3RAVEN", CarId = "C4", IsAvailable = true, Mileage = 45200 },
                new CarFleet { Vin = "SKO3OCT0000000002", RegistrationNumber = "KR10002", CarId = "C4", IsAvailable = true, Mileage = 58000 },
                new CarFleet { Vin = "SKO3OCT0000000003", RegistrationNumber = "KR10003", CarId = "C4", IsAvailable = false, Mileage = 92500 },

                // 2x Volkswagen (Golf 8 - C14, Passat 9 - C17)
                new CarFleet { Vin = "VW8GOLF0000000004", RegistrationNumber = "WA20004", CarId = "C14", IsAvailable = true, Mileage = 12300 },
                new CarFleet { Vin = "VW9PASS0000000005", RegistrationNumber = "WA20005", CarId = "C17", IsAvailable = true, Mileage = 850 },

                // 2x Audi (RS3 - C24, A4 - C25)
                new CarFleet { Vin = "AUDRS300000000006", RegistrationNumber = "PO30006", CarId = "C24", IsAvailable = true, Mileage = 5200 },
                new CarFleet { Vin = "AUDA4000000000007", RegistrationNumber = "PO30007", CarId = "C25", IsAvailable = true, Mileage = 24000 },

                // 3x Losowe (Volvo XC90 - C33, Kia Sportage - C29, Tesla 3 - C3)
                new CarFleet { Vin = "VOLXC900000000008", RegistrationNumber = "DW40008", CarId = "C33", IsAvailable = true, Mileage = 15000 },
                new CarFleet { Vin = "KIASPOR0000000009", RegistrationNumber = "GD50009", CarId = "C29", IsAvailable = false, Mileage = 33100 },
                new CarFleet { Vin = "TESMOD30000000010", RegistrationNumber = "EL60010", CarId = "C3", IsAvailable = true, Mileage = 21000 }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client { ClientID = 1, Username = "jkowalski", Password = "123", FirstName = "Jan", LastName = "Kowalski", Email = "jan@wp.pl", Phone = "111222333", City = "Kraków", Street = "Długa", HouseNumber = "5", PostalCode = "31-000", Country = "Poland" },
                new Client { ClientID = 2, Username = "anowak", Password = "123", FirstName = "Anna", LastName = "Nowak", Email = "anna@wp.pl", Phone = "444555666", City = "Warszawa", Street = "Prosta", HouseNumber = "10", PostalCode = "00-001", Country = "Poland" },
                new Client { ClientID = 3, Username = "mwojcik", Password = "123", FirstName = "Marek", LastName = "Wójcik", Email = "marek@wp.pl", Phone = "777888999", City = "Gdańsk", Street = "Morska", HouseNumber = "1", PostalCode = "80-000", Country = "Poland" },
                new Client { ClientID = 4, Username = "rlewandowski", Password = "999", FirstName = "Robert", LastName = "Lewandowski", Email = "rl9@bramki.pl", Phone = "999888777", City = "Warszawa", Street = "Złota", HouseNumber = "44", PostalCode = "00-001", Country = "Poland" },
                new Client { ClientID = 5, Username = "msklodowska", Password = "rad", FirstName = "Maria", LastName = "Skłodowska", Email = "maria@science.pl", Phone = "112233445", City = "Wrocław", Street = "Główna", HouseNumber = "102", PostalCode = "50-100", Country = "Poland" },
                new Client { ClientID = 6, Username = "zboniek", Password = "pzpn", FirstName = "Zbigniew", LastName = "Boniek", Email = "zibi@sport.pl", Phone = "666555444", City = "Łódź", Street = "Piotrkowska", HouseNumber = "15", PostalCode = "90-001", Country = "Poland" },
                new Client { ClientID = 7, Username = "kkrawczyk", Password = "rejs", FirstName = "Krzysztof", LastName = "Krawczyk", Email = "parostatek@muzyka.pl", Phone = "555000555", City = "Gdynia", Street = "Morska", HouseNumber = "7", PostalCode = "81-100", Country = "Poland" },
                new Client { ClientID = 8, Username = "dpodsiadlo", Password = "malomiasteczkowy", FirstName = "Dawid", LastName = "Podsiadło", Email = "dawid@koncert.pl", Phone = "123123123", City = "Dąbrowa Górnicza", Street = "Krótka", HouseNumber = "3", PostalCode = "41-300", Country = "Poland" }
            );

            modelBuilder.Entity<Worker>().HasData(
                new Worker { WorkerID = 1, Username = "admin", Password = "admin_password", FirstName = "Adam", LastName = "Kierownik", Position = "Manager" },
                new Worker { WorkerID = 2, Username = "staff1", Password = "staff_password", FirstName = "Ewa", LastName = "Pracownik", Position = "Sales Assistant" },
                new Worker { WorkerID = 3, Username = "staff2", Password = "staff_password", FirstName = "Piotr", LastName = "Serwisant", Position = "Technician" },
                new Worker { WorkerID = 4, Username = "mmostowiak", Password = "kartony", FirstName = "Marek", LastName = "Mostowiak", Position = "Specjalista ds. Wynajmu" },
                new Worker { WorkerID = 5, Username = "bkozidrak", Password = "diva", FirstName = "Beata", LastName = "Kozidrak", Position = "Recepcjonistka" },
                new Worker { WorkerID = 6, Username = "rmaklowicz", Password = "koper", FirstName = "Robert", LastName = "Makłowicz", Position = "Koordynator Logistyki" },
                new Worker { WorkerID = 7, Username = "mgessler", Password = "cebula", FirstName = "Magda", LastName = "Gessler", Position = "Kontroler Jakości Aut" },
                new Worker { WorkerID = 8, Username = "jdudek", Password = "dance", FirstName = "Jerzy", LastName = "Dudek", Position = "Manager Floty" }
            );

            modelBuilder.Entity<Reservation>().HasData(
                new Reservation { Id = 1, CarVin = "VIN12345678901234", ClientId = 1, WorkerId = 1, StartDate = new DateTime(2026, 03, 10), EndDate = new DateTime(2026, 03, 13), TotalPrice = 450 },
                new Reservation { Id = 2, CarVin = "VIN98765432109876", ClientId = 2, WorkerId = 2, StartDate = new DateTime(2026, 04, 01), EndDate = new DateTime(2026, 04, 04), TotalPrice = 900 },
                new Reservation { Id = 3, CarVin = "VIN00000000000000", ClientId = 3, WorkerId = 1, StartDate = new DateTime(2026, 05, 20), EndDate = new DateTime(2026, 05, 27), TotalPrice = 3150 },
                new Reservation { Id = 4, CarVin = "SKO3OCT0000000001", ClientId = 4, WorkerId = 4, StartDate = new DateTime(2026, 6, 1), EndDate = new DateTime(2026, 6, 10), TotalPrice = 1800 },
                new Reservation { Id = 5, CarVin = "VW8GOLF0000000004", ClientId = 5, WorkerId = 5, StartDate = new DateTime(2026, 6, 15), EndDate = new DateTime(2026, 6, 20), TotalPrice = 1050 },
                new Reservation { Id = 6, CarVin = "AUDRS300000000006", ClientId = 6, WorkerId = 6, StartDate = new DateTime(2026, 7, 1), EndDate = new DateTime(2026, 7, 3), TotalPrice = 1700 },
                new Reservation { Id = 7, CarVin = "VOLXC900000000008", ClientId = 7, WorkerId = 7, StartDate = new DateTime(2026, 8, 10), EndDate = new DateTime(2026, 8, 20), TotalPrice = 6500 },
                new Reservation { Id = 8, CarVin = "TESMOD30000000010", ClientId = 8, WorkerId = 8, StartDate = new DateTime(2026, 9, 5), EndDate = new DateTime(2026, 9, 7), TotalPrice = 900 }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Twój Connection String do bazy danych
                optionsBuilder.UseSqlServer("Server=localhost;Initial Catalog=Wypożyczalnia;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"); // Upewnij się, że ten Connection String jest poprawny i bezpieczny
            }
        }
    }
}
