using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class CalendarMigration5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairEvents_Repairs_RepairId",
                table: "CalendarEvents");

            migrationBuilder.DropIndex(
                name: "IX_RepairEvents_RepairId",
                table: "CalendarEvents");

            migrationBuilder.DropColumn(
                name: "RepairId",
                table: "CalendarEvents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepairId",
                table: "CalendarEvents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairEvents_RepairId",
                table: "CalendarEvents",
                column: "RepairId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairEvents_Repairs_RepairId",
                table: "CalendarEvents",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "RepairId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
