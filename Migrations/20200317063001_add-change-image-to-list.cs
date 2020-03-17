using Microsoft.EntityFrameworkCore.Migrations;

namespace AsseTS.Migrations
{
    public partial class addchangeimagetolist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Images",
                table: "Goods");

            migrationBuilder.AddColumn<string>(
                name: "_images",
                table: "Goods",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "_images",
                table: "Goods");

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Goods",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
