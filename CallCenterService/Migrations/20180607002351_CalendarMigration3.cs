using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CallCenterService.Migrations
{
    public partial class CalendarMigration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkTimeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkTime",
                columns: table => new
                {
                    WorkTimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FridayEnd = table.Column<DateTime>(nullable: false),
                    FridayStart = table.Column<DateTime>(nullable: false),
                    MondayEnd = table.Column<DateTime>(nullable: false),
                    MondayStart = table.Column<DateTime>(nullable: false),
                    SaturdayEnd = table.Column<DateTime>(nullable: false),
                    SaturdayStart = table.Column<DateTime>(nullable: false),
                    ServicerId = table.Column<string>(nullable: true),
                    SundayEnd = table.Column<DateTime>(nullable: false),
                    SundayStart = table.Column<DateTime>(nullable: false),
                    ThursdayEnd = table.Column<DateTime>(nullable: false),
                    ThursdayStart = table.Column<DateTime>(nullable: false),
                    TuesdayEnd = table.Column<DateTime>(nullable: false),
                    TuesdayStart = table.Column<DateTime>(nullable: false),
                    WednesdayEnd = table.Column<DateTime>(nullable: false),
                    WednesdayStart = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTime", x => x.WorkTimeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WorkTimeId",
                table: "AspNetUsers",
                column: "WorkTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkTime_WorkTimeId",
                table: "AspNetUsers",
                column: "WorkTimeId",
                principalTable: "WorkTime",
                principalColumn: "WorkTimeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkTime_WorkTimeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WorkTime");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WorkTimeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WorkTimeId",
                table: "AspNetUsers");
        }
    }
}
