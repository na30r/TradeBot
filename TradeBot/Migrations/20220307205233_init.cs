using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeBot.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Candles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candles",
                table: "Candles",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Candles",
                table: "Candles");

            migrationBuilder.RenameTable(
                name: "Candles",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ID");
        }
    }
}
