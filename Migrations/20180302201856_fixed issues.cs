using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class fixedissues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCustCapStyle",
                table: "FixedPriceMonths");

            migrationBuilder.DropColumn(
                name: "PlannedCostCapStyle",
                table: "FixedPriceMonths");

            migrationBuilder.AddColumn<int>(
                name: "ActualCostStyle",
                table: "FixedPriceMonths",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlannedCostStyle",
                table: "FixedPriceMonths",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualCostStyle",
                table: "FixedPriceMonths");

            migrationBuilder.DropColumn(
                name: "PlannedCostStyle",
                table: "FixedPriceMonths");

            migrationBuilder.AddColumn<int>(
                name: "ActualCustCapStyle",
                table: "FixedPriceMonths",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlannedCostCapStyle",
                table: "FixedPriceMonths",
                nullable: true);
        }
    }
}
