using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.Data.Migrations
{
    public partial class AddedPaymentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservings_BankAccounts_BankAccountId",
                table: "Reservings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservings_BankCards_BankCardId",
                table: "Reservings");

            migrationBuilder.DropIndex(
                name: "IX_Reservings_BankAccountId",
                table: "Reservings");

            migrationBuilder.DropIndex(
                name: "IX_Reservings_BankCardId",
                table: "Reservings");

            migrationBuilder.DropColumn(
                name: "BankAccountId",
                table: "Reservings");

            migrationBuilder.DropColumn(
                name: "BankCardId",
                table: "Reservings");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Reservings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankCardIdFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankAcountIdTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentDatas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentDatas");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Reservings");

            migrationBuilder.AddColumn<long>(
                name: "BankAccountId",
                table: "Reservings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankCardId",
                table: "Reservings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "Members",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_BankAccountId",
                table: "Reservings",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_BankCardId",
                table: "Reservings",
                column: "BankCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservings_BankAccounts_BankAccountId",
                table: "Reservings",
                column: "BankAccountId",
                principalTable: "BankAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservings_BankCards_BankCardId",
                table: "Reservings",
                column: "BankCardId",
                principalTable: "BankCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
