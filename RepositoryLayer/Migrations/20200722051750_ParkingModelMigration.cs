using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class ParkingModelMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParkingDetails",
                columns: table => new
                {
                    ReceiptNumber = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VehicalOwnerName = table.Column<string>(nullable: false),
                    VehicalNumber = table.Column<string>(nullable: false),
                    Brand = table.Column<string>(nullable: false),
                    Color = table.Column<string>(nullable: false),
                    DriverName = table.Column<string>(nullable: false),
                    ParkingSlot = table.Column<string>(nullable: true),
                    ParkingDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingDetails", x => x.ReceiptNumber);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParkingDetails");
        }
    }
}
