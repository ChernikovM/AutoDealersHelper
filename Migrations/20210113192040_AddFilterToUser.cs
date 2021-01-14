using Microsoft.EntityFrameworkCore.Migrations;

namespace AutoDealersHelper.Migrations
{
    public partial class AddFilterToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Fuel",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GearBox",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Mileage",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Model",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "Users",
                newName: "FilterString");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilterString",
                table: "Users",
                newName: "Year");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fuel",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GearBox",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mileage",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Model",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Volume",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
