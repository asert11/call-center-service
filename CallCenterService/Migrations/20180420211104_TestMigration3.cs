using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CallCenterService.Migrations
{
    public partial class TestMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServicerDescription",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Faults",
                newName: "ProductID");

            migrationBuilder.AlterColumn<int>(
                name: "ServicerId",
                table: "Repairs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "FaultId",
                table: "Repairs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ProductID",
                table: "Faults",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_FaultId",
                table: "Repairs",
                column: "FaultId");

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_ServicerId",
                table: "Repairs",
                column: "ServicerId");

            migrationBuilder.CreateIndex(
                name: "IX_Faults_ProductID",
                table: "Faults",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Faults_Products_ProductID",
                table: "Faults",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Faults_FaultId",
                table: "Repairs",
                column: "FaultId",
                principalTable: "Faults",
                principalColumn: "FaultId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Servicers_ServicerId",
                table: "Repairs",
                column: "ServicerId",
                principalTable: "Servicers",
                principalColumn: "ServicerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Faults_Products_ProductID",
                table: "Faults");

            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Faults_FaultId",
                table: "Repairs");

            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Servicers_ServicerId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_FaultId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_ServicerId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Faults_ProductID",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Faults",
                newName: "ProductId");

            migrationBuilder.AlterColumn<int>(
                name: "ServicerId",
                table: "Repairs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FaultId",
                table: "Repairs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Faults",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServicerDescription",
                table: "Faults",
                nullable: true);
        }
    }
}
