using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SimplePMServices.Migrations
{
    public partial class correctedprojectfortemplates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isTemplate",
                table: "Projects",
                newName: "IsTemplate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTemplate",
                table: "Projects",
                newName: "isTemplate");
        }
    }
}
