﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AsseTS.Migrations
{
    public partial class changeroletoRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "role",
                table: "User",
                newName: "Role");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "User",
                newName: "role");
        }
    }
}
