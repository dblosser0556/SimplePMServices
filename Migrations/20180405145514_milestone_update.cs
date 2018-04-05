using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class milestone_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhaseCompleteEstimate",
                table: "Milestones");

            migrationBuilder.AddColumn<double>(
                name: "PhaseCapitalEstimate",
                table: "Milestones",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PhaseExpenseEstimate",
                table: "Milestones",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhaseCapitalEstimate",
                table: "Milestones");

            migrationBuilder.DropColumn(
                name: "PhaseExpenseEstimate",
                table: "Milestones");

            migrationBuilder.AddColumn<double>(
                name: "PhaseCompleteEstimate",
                table: "Milestones",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
