using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplePMServices.Migrations
{
    public partial class updatephase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dashboard",
                table: "Phases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Phases",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dashboard",
                table: "Phases");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Phases");
        }
    }
}
