using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class UpdatingParkigModelForUnpark : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalAmount",
                table: "ParkingDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalTime",
                table: "ParkingDetails",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnparkDate",
                table: "ParkingDetails",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "ParkingDetails");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "ParkingDetails");

            migrationBuilder.DropColumn(
                name: "UnparkDate",
                table: "ParkingDetails");
        }
    }
}
