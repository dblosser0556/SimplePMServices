using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimplePMServices.Migrations
{
    public partial class updatestatusphase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dashboard",
                table: "Phases");

            migrationBuilder.AddColumn<bool>(
                name: "Dashboard",
                table: "Status",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Status",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dashboard",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "Status");

            migrationBuilder.AddColumn<bool>(
                name: "Dashboard",
                table: "Phases",
                nullable: false,
                defaultValue: false);
        }
    }
}
