using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeBot.Migrations
{
    public partial class candleAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Timeframe",
                table: "Candles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timeframe",
                table: "Candles");
        }
    }
}
