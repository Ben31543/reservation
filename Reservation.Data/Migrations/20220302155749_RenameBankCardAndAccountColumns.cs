using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.Data.Migrations
{
    public partial class RenameBankCardAndAccountColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankAccountToId",
                table: "Reservings");

            migrationBuilder.DropColumn(
                name: "BankCardFromId",
                table: "Reservings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BankAccountToId",
                table: "Reservings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankCardFromId",
                table: "Reservings",
                type: "bigint",
                nullable: true);
        }
    }
}
