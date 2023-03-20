using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    public partial class Moduleidforazure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<long>(
                name: "ModuleID",
                table: "GarageMenu",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GarageMenu_ModuleID",
                table: "GarageMenu",
                column: "ModuleID");

            migrationBuilder.AddForeignKey(
                name: "FK_GarageMenu_ClientModules_ModuleID",
                table: "GarageMenu",
                column: "ModuleID",
                principalTable: "ClientModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GarageMenu_ClientModules_ModuleID",
                table: "GarageMenu");

            migrationBuilder.DropIndex(
                name: "IX_GarageMenu_ModuleID",
                table: "GarageMenu");


            migrationBuilder.DropColumn(
                name: "ModuleID",
                table: "GarageMenu");

        }
    }
}
