using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class updateformonthstyle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualEffortStyle",
                table: "ResourceMonths",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlannedEffortStyle",
                table: "ResourceMonths",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActualCustCapStyle",
                table: "FixedPriceMonths",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlannedCostCapStyle",
                table: "FixedPriceMonths",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualEffortStyle",
                table: "ResourceMonths");

            migrationBuilder.DropColumn(
                name: "PlannedEffortStyle",
                table: "ResourceMonths");

            migrationBuilder.DropColumn(
                name: "ActualCustCapStyle",
                table: "FixedPriceMonths");

            migrationBuilder.DropColumn(
                name: "PlannedCostCapStyle",
                table: "FixedPriceMonths");
        }
    }
}
