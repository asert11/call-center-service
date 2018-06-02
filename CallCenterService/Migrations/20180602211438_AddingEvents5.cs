using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class AddingEvents5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Repairs_RepairId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "RepairEvents");

            migrationBuilder.RenameIndex(
                name: "IX_Events_RepairId",
                table: "RepairEvents",
                newName: "IX_RepairEvents_RepairId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairEvents",
                table: "RepairEvents",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairEvents_Repairs_RepairId",
                table: "RepairEvents",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "RepairId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairEvents_Repairs_RepairId",
                table: "RepairEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairEvents",
                table: "RepairEvents");

            migrationBuilder.RenameTable(
                name: "RepairEvents",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_RepairEvents_RepairId",
                table: "Events",
                newName: "IX_Events_RepairId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Repairs_RepairId",
                table: "Events",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "RepairId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
