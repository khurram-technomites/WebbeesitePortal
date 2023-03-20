using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class IntegrationSettingField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "DeliveryGateway",
                table: "IntegrationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSPassword",
                table: "IntegrationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SMSUsername",
                table: "IntegrationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalesEmail",
                table: "IntegrationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "IntegrationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "IntegrationSettings",
                type: "nvarchar(max)",
                nullable: true);

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.DropColumn(
                name: "DeliveryGateway",
                table: "IntegrationSettings");

            migrationBuilder.DropColumn(
                name: "SMSPassword",
                table: "IntegrationSettings");

            migrationBuilder.DropColumn(
                name: "SMSUsername",
                table: "IntegrationSettings");

            migrationBuilder.DropColumn(
                name: "SalesEmail",
                table: "IntegrationSettings");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "IntegrationSettings");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "IntegrationSettings");

           
        }
    }
}
