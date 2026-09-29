using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialBox.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDefaultToBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "FinancialBoxes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "FinancialBoxes");
        }
    }
}
