using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AsseTS.Migrations
{
    public partial class changehistoryschema2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_histories",
                table: "Goods");

            migrationBuilder.AddColumn<Guid>(
                name: "GoodsId",
                table: "Histories",
                type: "uniqueidentifier",
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
    }
}
