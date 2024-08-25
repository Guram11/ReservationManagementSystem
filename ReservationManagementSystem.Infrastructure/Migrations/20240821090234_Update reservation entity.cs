using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatereservationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomTypeId",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "ReservationRoomTimelines",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Reservations",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Date",
                table: "ReservationRoomTimelines",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
