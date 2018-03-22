using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class fixed_group_to_budget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Groups_GroupId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_GroupId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Budgets");

            migrationBuilder.CreateIndex(
                name: "IX_GroupBudgets_GroupId",
                table: "GroupBudgets",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupBudgets_Groups_GroupId",
                table: "GroupBudgets",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupBudgets_Groups_GroupId",
                table: "GroupBudgets");

            migrationBuilder.DropIndex(
                name: "IX_GroupBudgets_GroupId",
                table: "GroupBudgets");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Budgets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_GroupId",
                table: "Budgets",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Groups_GroupId",
                table: "Budgets",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
