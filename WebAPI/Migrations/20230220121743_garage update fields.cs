using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class garageupdatefields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "IsAwardAllowed",
                table: "Garages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomerAppoinmentAllowed",
                table: "Garages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMenusAllowed",
                table: "Garages",
                type: "bit",
                nullable: false,
                defaultValue: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "IsAwardAllowed",
                table: "Garages");

            migrationBuilder.DropColumn(
                name: "IsCustomerAppoinmentAllowed",
                table: "Garages");

            migrationBuilder.DropColumn(
                name: "IsMenusAllowed",
                table: "Garages");

        }
    }
}
