using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class ClientMOdulepurchasefiledadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<decimal>(
                name: "AmountToBePaid",
                table: "ClientModulePurchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropColumn(
                name: "AmountToBePaid",
                table: "ClientModulePurchases");

          
        }
    }
}
