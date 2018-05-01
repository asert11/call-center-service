using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CallCenterService.Migrations
{
    public partial class TestMigration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientFirstName",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ClientSecondName",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "PaymentData",
                table: "Faults",
                newName: "ServicerDescription");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Faults",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Faults",
                newName: "FaultId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "Faults",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ClientDescription",
                table: "Faults",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Faults",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Adress = table.Column<string>(nullable: false),
                    FaultId = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: false),
                    SecondName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Clients_Faults_FaultId",
                        column: x => x.FaultId,
                        principalTable: "Faults",
                        principalColumn: "FaultId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    RepairId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    FaultId = table.Column<int>(nullable: false),
                    ServicerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.RepairId);
                });

            migrationBuilder.CreateTable(
                name: "Servicers",
                columns: table => new
                {
                    ServicerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    SecondName = table.Column<string>(nullable: false),
                    Specialization = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicers", x => x.ServicerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clients_FaultId",
                table: "Clients",
                column: "FaultId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "Servicers");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "ClientDescription",
                table: "Faults");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Faults");

            migrationBuilder.RenameColumn(
                name: "ServicerDescription",
                table: "Faults",
                newName: "PaymentData");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Faults",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "FaultId",
                table: "Faults",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "ClientFirstName",
                table: "Faults",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientSecondName",
                table: "Faults",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Faults",
                nullable: true);
        }
    }
}
