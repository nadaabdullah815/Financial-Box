using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialBox.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDebtBoxToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDebtBox",
                table: "FinancialBoxes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDebtBox",
                table: "FinancialBoxes");
        }
    }
}
