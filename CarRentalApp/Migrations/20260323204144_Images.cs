using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class Images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C1",
                column: "ImagePath",
                value: "/Images/ToyotaCorolla.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C14",
                column: "ImagePath",
                value: "/Images/VwGolf8.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C17",
                column: "ImagePath",
                value: "/Images/VwPassat9.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C2",
                column: "ImagePath",
                value: "/Images/BmwModel3.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C24",
                column: "ImagePath",
                value: "/Images/AudiRS3.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C25",
                column: "ImagePath",
                value: "/Images/AudiA4.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C29",
                column: "ImagePath",
                value: "/Images/KiaSportage.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C3",
                column: "ImagePath",
                value: "/Images/TeslaModel3.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C33",
                column: "ImagePath",
                value: "/Images/VolvoXC90.png");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C4",
                column: "ImagePath",
                value: "/Images/SkodaOctavia3.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C1",
                column: "ImagePath",
                value: "corolla.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C14",
                column: "ImagePath",
                value: "golf8.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C17",
                column: "ImagePath",
                value: "passat9.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C2",
                column: "ImagePath",
                value: "bmw3.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C24",
                column: "ImagePath",
                value: "audirs3.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C25",
                column: "ImagePath",
                value: "audia4.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C29",
                column: "ImagePath",
                value: "sportage.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C3",
                column: "ImagePath",
                value: "tesla3.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C33",
                column: "ImagePath",
                value: "xc90.jpg");

            migrationBuilder.UpdateData(
                table: "Cars",
                keyColumn: "IdCar",
                keyValue: "C4",
                column: "ImagePath",
                value: "octavia3.jpg");
        }
    }
}
