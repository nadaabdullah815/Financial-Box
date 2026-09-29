using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialBox.Migrations
{
    /// <inheritdoc />
    public partial class AddTransfers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromBoxId = table.Column<int>(type: "int", nullable: false),
                    ToBoxId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_FinancialBoxes_FromBoxId",
                        column: x => x.FromBoxId,
                        principalTable: "FinancialBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transfers_FinancialBoxes_ToBoxId",
                        column: x => x.ToBoxId,
                        principalTable: "FinancialBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_FromBoxId",
                table: "Transfers",
                column: "FromBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_ToBoxId",
                table: "Transfers",
                column: "ToBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transfers");
        }
    }
}
