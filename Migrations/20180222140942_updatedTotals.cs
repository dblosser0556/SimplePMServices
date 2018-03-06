using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class updatedTotals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalActualEffort",
                table: "ResourceMonths");

            migrationBuilder.DropColumn(
                name: "TotalPlannedEffort",
                table: "ResourceMonths");

            migrationBuilder.AddColumn<double>(
                name: "TotalActualEffort",
                table: "Resources",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPlannedEffort",
                table: "Resources",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalActualCost",
                table: "FixedPrices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPlannedCost",
                table: "FixedPrices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalActualEffort",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "TotalPlannedEffort",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "TotalActualCost",
                table: "FixedPrices");

            migrationBuilder.DropColumn(
                name: "TotalPlannedCost",
                table: "FixedPrices");

            migrationBuilder.AddColumn<double>(
                name: "TotalActualEffort",
                table: "ResourceMonths",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPlannedEffort",
                table: "ResourceMonths",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
