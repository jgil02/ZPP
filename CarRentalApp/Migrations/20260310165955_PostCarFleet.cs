using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class PostCarFleet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Cars_CarVin",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "Vin",
                table: "Cars",
                newName: "IdCar");

            migrationBuilder.AlterColumn<string>(
                name: "CarVin",
                table: "Reservations",
                type: "nvarchar(17)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Seats",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CarFleet",
                columns: table => new
                {
                    Vin = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CarId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarFleet", x => x.Vin);
                    table.ForeignKey(
                        name: "FK_CarFleet_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "IdCar",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarFleet_CarId",
                table: "CarFleet",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_CarFleet_CarVin",
                table: "Reservations",
                column: "CarVin",
                principalTable: "CarFleet",
                principalColumn: "Vin",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_CarFleet_CarVin",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "CarFleet");

            migrationBuilder.DropColumn(
                name: "Seats",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "IdCar",
                table: "Cars",
                newName: "Vin");

            migrationBuilder.AlterColumn<string>(
                name: "CarVin",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(17)");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Cars_CarVin",
                table: "Reservations",
                column: "CarVin",
                principalTable: "Cars",
                principalColumn: "Vin",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
