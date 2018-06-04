using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class CalendarMigration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CalendarEventEventId",
                table: "Repairs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_CalendarEventEventId",
                table: "Repairs",
                column: "CalendarEventEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_CalendarEvents_CalendarEventEventId",
                table: "Repairs",
                column: "CalendarEventEventId",
                principalTable: "CalendarEvents",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_CalendarEvents_CalendarEventEventId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_CalendarEventEventId",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "CalendarEventEventId",
                table: "Repairs");
        }
    }
}
