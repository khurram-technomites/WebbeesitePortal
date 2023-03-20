using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class Integrationsetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.RenameColumn(
                name: "DeliveryGateway",
                table: "IntegrationSettings",
                newName: "DLR");

           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.RenameColumn(
                name: "DLR",
                table: "IntegrationSettings",
                newName: "DeliveryGateway");

          
        }
    }
}
