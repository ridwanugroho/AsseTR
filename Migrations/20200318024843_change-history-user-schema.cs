using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AsseTS.Migrations
{
    public partial class changehistoryuserschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Histories");

            migrationBuilder.AddColumn<Guid>(
                name: "InChargeId",
                table: "Histories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Histories_InChargeId",
                table: "Histories",
                column: "InChargeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_User_InChargeId",
                table: "Histories",
                column: "InChargeId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_User_InChargeId",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_InChargeId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "InChargeId",
                table: "Histories");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Histories",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
