using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoDealersHelper.Migrations
{
    public partial class ChatStateIdColumn_AddedToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatStateId",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Volume",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatStateId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Users");
        }
    }
}
