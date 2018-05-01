using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class TestMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Faults_FaultId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_FaultId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "FaultId",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Faults",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ClientId",
                table: "Faults",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Clients_ClientId",
                table: "Faults",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Clients_ClientId",
                table: "Faults");

            migrationBuilder.DropIndex(
                name: "IX_Faults_ClientId",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Faults");

            migrationBuilder.AddColumn<int>(
                name: "FaultId",
                table: "Clients",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_FaultId",
                table: "Clients",
                column: "FaultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Faults_FaultId",
                table: "Clients",
                column: "FaultId",
                principalTable: "Faults",
                principalColumn: "FaultId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
