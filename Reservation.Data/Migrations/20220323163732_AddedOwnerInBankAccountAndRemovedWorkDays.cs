using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.Data.Migrations
{
    public partial class AddedOwnerInBankAccountAndRemovedWorkDays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ServiceMembers");

            migrationBuilder.DropColumn(
                name: "WorkDays",
                table: "ServiceMemberBranches");

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "BankAccounts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Owner",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ServiceMembers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkDays",
                table: "ServiceMemberBranches",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
