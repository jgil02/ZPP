using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class AfterChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarFleet_Cars_CarId",
                table: "CarFleet");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_CarFleet_CarVin",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarFleet",
                table: "CarFleet");

            migrationBuilder.RenameTable(
                name: "CarFleet",
                newName: "CarFleets");

            migrationBuilder.RenameIndex(
                name: "IX_CarFleet_CarId",
                table: "CarFleets",
                newName: "IX_CarFleets_CarId");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Clients",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Clients",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarFleets",
                table: "CarFleets",
                column: "Vin");

            migrationBuilder.AddForeignKey(
                name: "FK_CarFleets_Cars_CarId",
                table: "CarFleets",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "IdCar",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_CarFleets_CarVin",
                table: "Reservations",
                column: "CarVin",
                principalTable: "CarFleets",
                principalColumn: "Vin",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarFleets_Cars_CarId",
                table: "CarFleets");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_CarFleets_CarVin",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarFleets",
                table: "CarFleets");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Clients");

            migrationBuilder.RenameTable(
                name: "CarFleets",
                newName: "CarFleet");

            migrationBuilder.RenameIndex(
                name: "IX_CarFleets_CarId",
                table: "CarFleet",
                newName: "IX_CarFleet_CarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarFleet",
                table: "CarFleet",
                column: "Vin");

            migrationBuilder.AddForeignKey(
                name: "FK_CarFleet_Cars_CarId",
                table: "CarFleet",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "IdCar",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_CarFleet_CarVin",
                table: "Reservations",
                column: "CarVin",
                principalTable: "CarFleet",
                principalColumn: "Vin",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Clients_ClientId",
                table: "Reservations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
