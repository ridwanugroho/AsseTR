using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AsseTS.Migrations
{
    public partial class changebrandstringtoBrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Goods");

            migrationBuilder.AddColumn<Guid>(
                name: "BrandId",
                table: "Goods",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_BrandId",
                table: "Goods",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_Brands_BrandId",
                table: "Goods",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Goods_Brands_BrandId",
                table: "Goods");

            migrationBuilder.DropIndex(
                name: "IX_Goods_BrandId",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Goods");

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Goods",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
