using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    IdCar = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PricePerDay = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeatsCount = table.Column<int>(type: "int", nullable: false),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DoorsCount = table.Column<int>(type: "int", nullable: false),
                    GearboxType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FuelType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BodyType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TrunkCapacity = table.Column<int>(type: "int", nullable: false),
                    EngineCapacity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.IdCar);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientID);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    WorkerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Position = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.WorkerID);
                });

            migrationBuilder.CreateTable(
                name: "CarFleets",
                columns: table => new
                {
                    Vin = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CarId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFleets", x => x.Vin);
                    table.ForeignKey(
                        name: "FK_CarFleets_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "IdCar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarVin = table.Column<string>(type: "nvarchar(17)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    WorkerId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_CarFleets_CarVin",
                        column: x => x.CarVin,
                        principalTable: "CarFleets",
                        principalColumn: "Vin",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Workers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Workers",
                        principalColumn: "WorkerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "IdCar", "BodyType", "Brand", "DoorsCount", "EngineCapacity", "FuelType", "GearboxType", "ImagePath", "Model", "PricePerDay", "SeatsCount", "Segment", "TrunkCapacity" },
                values: new object[,]
                {
                    { "C1", "Sedan", "Toyota", 5, 1.8, "Hybrid", "Manual", "corolla.jpg", "Corolla", 150m, 5, "C", 470 },
                    { "C10", "Hatchback", "Volkswagen", 5, 1.2, "Petrol", "Manual", "polo5.jpg", "Polo 5", 110m, 5, "B", 280 },
                    { "C11", "Hatchback", "Volkswagen", 5, 1.0, "Petrol", "Manual", "polo6.jpg", "Polo 6", 135m, 5, "B", 351 },
                    { "C12", "Hatchback", "Volkswagen", 5, 1.3999999999999999, "Petrol", "Manual", "golf6.jpg", "Golf 6", 150m, 5, "C", 350 },
                    { "C13", "Hatchback", "Volkswagen", 5, 1.6000000000000001, "Diesel", "Automatic", "golf7.jpg", "Golf 7", 170m, 5, "C", 380 },
                    { "C14", "Hatchback", "Volkswagen", 5, 1.5, "Hybrid", "Automatic", "golf8.jpg", "Golf 8", 210m, 5, "C", 381 },
                    { "C15", "Sedan", "Volkswagen", 4, 2.0, "Diesel", "Manual", "passat7.jpg", "Passat 7", 190m, 5, "D", 565 },
                    { "C16", "Combi", "Volkswagen", 5, 2.0, "Diesel", "Automatic", "passat8.jpg", "Passat 8", 230m, 5, "D", 650 },
                    { "C17", "Combi", "Volkswagen", 5, 2.0, "Diesel", "Automatic", "passat9.jpg", "Passat 9", 320m, 5, "D", 690 },
                    { "C18", "Hatchback", "Seat", 5, 1.3999999999999999, "Petrol", "Manual", "leon3.jpg", "Leon 3", 160m, 5, "C", 380 },
                    { "C19", "Hatchback", "Seat", 5, 1.5, "Petrol", "Automatic", "leon4.jpg", "Leon 4", 200m, 5, "C", 380 },
                    { "C2", "Sedan", "BMW", 4, 2.0, "Petrol", "Automatic", "bmw3.jpg", "3 Series", 300m, 5, "D", 480 },
                    { "C20", "Hatchback", "Seat", 5, 1.2, "Petrol", "Manual", "ibiza4.jpg", "Ibiza 4", 110m, 5, "B", 292 },
                    { "C21", "Hatchback", "Seat", 5, 1.0, "Petrol", "Manual", "ibiza5.jpg", "Ibiza 5", 130m, 5, "B", 355 },
                    { "C22", "Hatchback", "Audi", 5, 1.5, "Petrol", "Automatic", "audia3.jpg", "A3", 250m, 5, "C", 380 },
                    { "C23", "Hatchback", "Audi", 5, 2.0, "Petrol", "Automatic", "audis3.jpg", "S3", 500m, 5, "C", 325 },
                    { "C24", "Hatchback", "Audi", 5, 2.5, "Petrol", "Automatic", "audirs3.jpg", "RS3", 850m, 5, "C", 282 },
                    { "C25", "Sedan", "Audi", 4, 2.0, "Diesel", "Automatic", "audia4.jpg", "A4", 300m, 5, "D", 460 },
                    { "C26", "Sedan", "Audi", 4, 3.0, "Diesel", "Automatic", "audis4.jpg", "S4", 600m, 5, "D", 420 },
                    { "C27", "Combi", "Audi", 5, 2.8999999999999999, "Petrol", "Automatic", "audirs4.jpg", "RS4", 1000m, 5, "D", 495 },
                    { "C28", "Hatchback", "Opel", 5, 1.2, "Petrol", "Manual", "corsa.jpg", "Corsa", 115m, 5, "B", 309 },
                    { "C29", "SUV", "Kia", 5, 1.6000000000000001, "Petrol", "Manual", "sportage.jpg", "Sportage", 240m, 5, "SUV", 591 },
                    { "C3", "Sedan", "Tesla", 4, 0.0, "Electric", "Automatic", "tesla3.jpg", "Model 3", 450m, 5, "C", 542 },
                    { "C30", "Crossover", "Kia", 5, 1.2, "Petrol", "Manual", "stonic.jpg", "Stonic", 160m, 5, "B-SUV", 332 },
                    { "C31", "SUV", "Volvo", 5, 2.0, "Hybrid", "Automatic", "xc40.jpg", "XC40", 350m, 5, "SUV", 452 },
                    { "C32", "SUV", "Volvo", 5, 2.0, "Diesel", "Automatic", "xc60.jpg", "XC60", 450m, 5, "SUV", 483 },
                    { "C33", "SUV", "Volvo", 5, 2.0, "Diesel", "Automatic", "xc90.jpg", "XC90", 650m, 7, "SUV", 640 },
                    { "C34", "Hatchback", "Renault", 5, 1.0, "Petrol", "Manual", "clio.jpg", "Clio", 120m, 5, "B", 391 },
                    { "C4", "Liftback", "Skoda", 5, 1.3999999999999999, "Petrol", "Automatic", "octavia3.jpg", "Octavia 3", 180m, 5, "C", 610 },
                    { "C5", "Combi", "Skoda", 5, 2.0, "Diesel", "Automatic", "octavia4.jpg", "Octavia 4", 220m, 5, "C", 600 },
                    { "C6", "Hatchback", "Skoda", 5, 1.0, "Petrol", "Manual", "fabia3.jpg", "Fabia 3", 120m, 5, "B", 330 },
                    { "C7", "Hatchback", "Skoda", 5, 1.0, "Petrol", "Manual", "fabia4.jpg", "Fabia 4", 140m, 5, "B", 380 },
                    { "C8", "Sedan", "Skoda", 5, 2.0, "Diesel", "Automatic", "superb3.jpg", "Superb 3", 280m, 5, "D", 625 },
                    { "C9", "Combi", "Skoda", 5, 2.0, "Diesel", "Automatic", "superb4.jpg", "Superb 4", 350m, 5, "D", 690 }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientID", "ApartmentNumber", "City", "Country", "Email", "FirstName", "HouseNumber", "LastName", "Password", "Phone", "PostalCode", "Street", "Username" },
                values: new object[,]
                {
                    { 1, null, "Kraków", "Poland", "jan@wp.pl", "Jan", "5", "Kowalski", "123", "111222333", "31-000", "Długa", "jkowalski" },
                    { 2, null, "Warszawa", "Poland", "anna@wp.pl", "Anna", "10", "Nowak", "123", "444555666", "00-001", "Prosta", "anowak" },
                    { 3, null, "Gdańsk", "Poland", "marek@wp.pl", "Marek", "1", "Wójcik", "123", "777888999", "80-000", "Morska", "mwojcik" },
                    { 4, null, "Warszawa", "Poland", "rl9@bramki.pl", "Robert", "44", "Lewandowski", "999", "999888777", "00-001", "Złota", "rlewandowski" },
                    { 5, null, "Wrocław", "Poland", "maria@science.pl", "Maria", "102", "Skłodowska", "rad", "112233445", "50-100", "Główna", "msklodowska" },
                    { 6, null, "Łódź", "Poland", "zibi@sport.pl", "Zbigniew", "15", "Boniek", "pzpn", "666555444", "90-001", "Piotrkowska", "zboniek" },
                    { 7, null, "Gdynia", "Poland", "parostatek@muzyka.pl", "Krzysztof", "7", "Krawczyk", "rejs", "555000555", "81-100", "Morska", "kkrawczyk" },
                    { 8, null, "Dąbrowa Górnicza", "Poland", "dawid@koncert.pl", "Dawid", "3", "Podsiadło", "malomiasteczkowy", "123123123", "41-300", "Krótka", "dpodsiadlo" }
                });

            migrationBuilder.InsertData(
                table: "Workers",
                columns: new[] { "WorkerID", "FirstName", "LastName", "Password", "Position", "Username" },
                values: new object[,]
                {
                    { 1, "Adam", "Kierownik", "admin_password", "Manager", "admin" },
                    { 2, "Ewa", "Pracownik", "staff_password", "Sales Assistant", "staff1" },
                    { 3, "Piotr", "Serwisant", "staff_password", "Technician", "staff2" },
                    { 4, "Marek", "Mostowiak", "kartony", "Specjalista ds. Wynajmu", "mmostowiak" },
                    { 5, "Beata", "Kozidrak", "diva", "Recepcjonistka", "bkozidrak" },
                    { 6, "Robert", "Makłowicz", "koper", "Koordynator Logistyki", "rmaklowicz" },
                    { 7, "Magda", "Gessler", "cebula", "Kontroler Jakości Aut", "mgessler" },
                    { 8, "Jerzy", "Dudek", "dance", "Manager Floty", "jdudek" }
                });

            migrationBuilder.InsertData(
                table: "CarFleets",
                columns: new[] { "Vin", "CarId", "IsAvailable", "Mileage", "RegistrationNumber" },
                values: new object[,]
                {
                    { "AUDA4000000000007", "C25", true, 24000, "PO30007" },
                    { "AUDRS300000000006", "C24", true, 5200, "PO30006" },
                    { "KIASPOR0000000009", "C29", false, 33100, "GD50009" },
                    { "SKO3OCT0000000001", "C4", true, 45200, "S3RAVEN" },
                    { "SKO3OCT0000000002", "C4", true, 58000, "KR10002" },
                    { "SKO3OCT0000000003", "C4", false, 92500, "KR10003" },
                    { "TESMOD30000000010", "C3", true, 21000, "EL60010" },
                    { "VIN00000000000000", "C3", false, 1200, "PO99999" },
                    { "VIN12345678901234", "C1", true, 15000, "KR12345" },
                    { "VIN98765432109876", "C2", true, 5000, "WA54321" },
                    { "VOLXC900000000008", "C33", true, 15000, "DW40008" },
                    { "VW8GOLF0000000004", "C14", true, 12300, "WA20004" },
                    { "VW9PASS0000000005", "C17", true, 850, "WA20005" }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CarVin", "ClientId", "EndDate", "StartDate", "TotalPrice", "WorkerId" },
                values: new object[,]
                {
                    { 1, "VIN12345678901234", 1, new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 450m, 1 },
                    { 2, "VIN98765432109876", 2, new DateTime(2026, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 900m, 2 },
                    { 3, "VIN00000000000000", 3, new DateTime(2026, 5, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 3150m, 1 },
                    { 4, "SKO3OCT0000000001", 4, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1800m, 4 },
                    { 5, "VW8GOLF0000000004", 5, new DateTime(2026, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1050m, 5 },
                    { 6, "AUDRS300000000006", 6, new DateTime(2026, 7, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1700m, 6 },
                    { 7, "VOLXC900000000008", 7, new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 6500m, 7 },
                    { 8, "TESMOD30000000010", 8, new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 900m, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarFleets_CarId",
                table: "CarFleets",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CarVin",
                table: "Reservations",
                column: "CarVin");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ClientId",
                table: "Reservations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_WorkerId",
                table: "Reservations",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "CarFleets");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "Cars");
        }
    }
}
