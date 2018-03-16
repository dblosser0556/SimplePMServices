using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class correctedhierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Left",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Right",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "Lft",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rgt",
                table: "Groups",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lft",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Rgt",
                table: "Groups");

            migrationBuilder.AddColumn<int>(
                name: "Left",
                table: "Groups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Right",
                table: "Groups",
                nullable: true);
        }
    }
}
