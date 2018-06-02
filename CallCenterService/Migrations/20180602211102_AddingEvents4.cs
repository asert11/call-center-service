using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class AddingEvents4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepairId",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_RepairId",
                table: "Events",
                column: "RepairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Repairs_RepairId",
                table: "Events",
                column: "RepairId",
                principalTable: "Repairs",
                principalColumn: "RepairId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Repairs_RepairId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_RepairId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "RepairId",
                table: "Events");
        }
    }
}
