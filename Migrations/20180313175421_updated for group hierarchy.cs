using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class updatedforgrouphierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Groups_GroupId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_GroupId",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "Left",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "LevelDesc",
                table: "Groups",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LevelId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Right",
                table: "Groups",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Left",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LevelDesc",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LevelId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Right",
                table: "Groups");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_GroupId",
                table: "Projects",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Groups_GroupId",
                table: "Projects",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
