using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AsseTS.Migrations
{
    public partial class changehistoryschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_histories",
                table: "Goods");

            migrationBuilder.AddColumn<Guid>(
                name: "GoodsId",
                table: "Histories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Histories_GoodsId",
                table: "Histories",
                column: "GoodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Histories_Goods_GoodsId",
                table: "Histories",
                column: "GoodsId",
                principalTable: "Goods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Histories_Goods_GoodsId",
                table: "Histories");

            migrationBuilder.DropIndex(
                name: "IX_Histories_GoodsId",
                table: "Histories");

            migrationBuilder.DropColumn(
                name: "GoodsId",
                table: "Histories");

            migrationBuilder.AddColumn<string>(
                name: "_histories",
                table: "Goods",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
