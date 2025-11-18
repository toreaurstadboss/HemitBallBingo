using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HemitBallBingo2025.Data.Migrations
{
    /// <inheritdoc />
    public partial class TicketMoreProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDrawn",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PrizeNumber",
                table: "Tickets",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDrawn",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PrizeNumber",
                table: "Tickets");
        }
    }
}
