using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class CalendarMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ResourceId",
                table: "CalendarEvents",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ResourceId",
                table: "CalendarEvents",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
