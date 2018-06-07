using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class CalendarMigration7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarEvents_Repairs_RepairId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_CalendarEvents_RepairId",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "RepairId",
                table: "CalendarEvents");

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

            migrationBuilder.AddColumn<int>(
                name: "RepairId",
                table: "CalendarEvents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEvents_RepairId",
                table: "CalendarEvents",
                column: "RepairId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_Repairs_RepairId",
                table: "CalendarEvents",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "RepairId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
