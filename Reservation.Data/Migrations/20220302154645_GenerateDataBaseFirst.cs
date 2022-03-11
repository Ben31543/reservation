using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reservation.Data.Migrations
{
    public partial class GenerateDataBaseFirst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)                                               
        {                                                                                                         
            migrationBuilder.CreateTable(               
                name: "Banks",                      
                columns: table => new
                {                            
                    Id = table.Column<long>(type: "bigint", nullable: false)                              
                        .Annotation("SqlServer:Identity", "1, 1"),                                     
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)                                                           
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BankId = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankCards",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ValidThru = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Owner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BankId = table.Column<long>(type: "bigint", nullable: false),
                    CVV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAttached = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankCards_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMembers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InstagramUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrdersCount = table.Column<int>(type: "int", nullable: false),
                    ViewsCount = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AcceptsOnlinePayment = table.Column<bool>(type: "bit", nullable: false),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceMembers_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankCardId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_BankCards_BankCardId",
                        column: x => x.BankCardId,
                        principalTable: "BankCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    ServiceMemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dishes_ServiceMembers_ServiceMemberId",
                        column: x => x.ServiceMemberId,
                        principalTable: "ServiceMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceMemberBranches",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceMemberId = table.Column<long>(type: "bigint", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    WorkDays = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TablesSchema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceMemberBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceMemberBranches_ServiceMembers_ServiceMemberId",
                        column: x => x.ServiceMemberId,
                        principalTable: "ServiceMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceMemberBranchId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceMemberId = table.Column<long>(type: "bigint", nullable: false),
                    BankCardFromId = table.Column<long>(type: "bigint", nullable: true),
                    BankAccountToId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsOnlinePayment = table.Column<bool>(type: "bit", nullable: false),
                    Tables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dishes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsTakeOut = table.Column<bool>(type: "bit", nullable: false),
                    BankCardId = table.Column<long>(type: "bigint", nullable: true),
                    BankAccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservings_BankAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalTable: "BankAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservings_BankCards_BankCardId",
                        column: x => x.BankCardId,
                        principalTable: "BankCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservings_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservings_ServiceMemberBranches_ServiceMemberBranchId",
                        column: x => x.ServiceMemberBranchId,
                        principalTable: "ServiceMemberBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservings_ServiceMembers_ServiceMemberId",
                        column: x => x.ServiceMemberId,
                        principalTable: "ServiceMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_BankId",
                table: "BankAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankCards_BankId",
                table: "BankCards",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_ServiceMemberId",
                table: "Dishes",
                column: "ServiceMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_BankCardId",
                table: "Members",
                column: "BankCardId",
                unique: true,
                filter: "[BankCardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_BankAccountId",
                table: "Reservings",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_BankCardId",
                table: "Reservings",
                column: "BankCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_MemberId",
                table: "Reservings",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_ServiceMemberBranchId",
                table: "Reservings",
                column: "ServiceMemberBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservings_ServiceMemberId",
                table: "Reservings",
                column: "ServiceMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMemberBranches_ServiceMemberId",
                table: "ServiceMemberBranches",
                column: "ServiceMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceMembers_BankAccountId",
                table: "ServiceMembers",
                column: "BankAccountId",
                unique: true,
                filter: "[BankAccountId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Reservings");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "ServiceMemberBranches");

            migrationBuilder.DropTable(
                name: "BankCards");

            migrationBuilder.DropTable(
                name: "ServiceMembers");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Banks");
        }
    }
}
