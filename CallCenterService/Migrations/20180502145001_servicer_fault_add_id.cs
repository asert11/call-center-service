using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CallCenterService.Migrations
{
    public partial class servicer_fault_add_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicerFault",
                table: "ServicerFault");

            migrationBuilder.DropColumn(
                name: "IdFault",
                table: "ServicerFault");

            migrationBuilder.AddColumn<int>(
                name: "IdFault",
                table: "ServicerFault",
                nullable: false,
                defaultValue: null);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ServicerFault",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicerFault",
                table: "ServicerFault",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EventHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicerFault",
                table: "ServicerFault");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ServicerFault");

            migrationBuilder.AlterColumn<int>(
                name: "IdFault",
                table: "ServicerFault",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicerFault",
                table: "ServicerFault",
                column: "IdFault");
        }
    }
}
