using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoDealersHelper.Migrations
{
    public partial class RenameChatStateIdInUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatStateId",
                table: "Users",
                newName: "ChatStateString");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatStateString",
                table: "Users",
                newName: "ChatStateId");
        }
    }
}
