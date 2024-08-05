using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class revertingRateRoomTypeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateTimelines_RateRoomTypes_RateRoomTypeId",
                table: "RateTimelines");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationRooms_RateRoomTypes_RateRoomTypeId",
                table: "ReservationRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationRoomServices",
                table: "ReservationRoomServices");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRoomServices_HotelServiceId",
                table: "ReservationRoomServices");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRooms_RateRoomTypeId",
                table: "ReservationRooms");

            migrationBuilder.DropIndex(
                name: "IX_RateTimelines_RateRoomTypeId",
                table: "RateTimelines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateRoomTypes",
                table: "RateRoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_RateRoomTypes_RateId",
                table: "RateRoomTypes");

            migrationBuilder.RenameColumn(
                name: "RateRoomTypeId",
                table: "ReservationRooms",
                newName: "RoomTypeId");

            migrationBuilder.RenameColumn(
                name: "RateRoomTypeId",
                table: "RateTimelines",
                newName: "RoomTypeId");

            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "ReservationRooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RateId",
                table: "RateTimelines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationRoomServices",
                table: "ReservationRoomServices",
                columns: new[] { "HotelServiceId", "ReservationRoomId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateRoomTypes",
                table: "RateRoomTypes",
                columns: new[] { "RateId", "RoomTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_RateId_RoomTypeId",
                table: "ReservationRooms",
                columns: new[] { "RateId", "RoomTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_RateTimelines_RateId_RoomTypeId",
                table: "RateTimelines",
                columns: new[] { "RateId", "RoomTypeId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RateTimelines_RateRoomTypes_RateId_RoomTypeId",
                table: "RateTimelines",
                columns: new[] { "RateId", "RoomTypeId" },
                principalTable: "RateRoomTypes",
                principalColumns: new[] { "RateId", "RoomTypeId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationRooms_RateRoomTypes_RateId_RoomTypeId",
                table: "ReservationRooms",
                columns: new[] { "RateId", "RoomTypeId" },
                principalTable: "RateRoomTypes",
                principalColumns: new[] { "RateId", "RoomTypeId" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateTimelines_RateRoomTypes_RateId_RoomTypeId",
                table: "RateTimelines");

            migrationBuilder.DropForeignKey(
                name: "FK_ReservationRooms_RateRoomTypes_RateId_RoomTypeId",
                table: "ReservationRooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationRoomServices",
                table: "ReservationRoomServices");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRooms_RateId_RoomTypeId",
                table: "ReservationRooms");

            migrationBuilder.DropIndex(
                name: "IX_RateTimelines_RateId_RoomTypeId",
                table: "RateTimelines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RateRoomTypes",
                table: "RateRoomTypes");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "ReservationRooms");

            migrationBuilder.DropColumn(
                name: "RateId",
                table: "RateTimelines");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "ReservationRooms",
                newName: "RateRoomTypeId");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "RateTimelines",
                newName: "RateRoomTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationRoomServices",
                table: "ReservationRoomServices",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RateRoomTypes",
                table: "RateRoomTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRoomServices_HotelServiceId",
                table: "ReservationRoomServices",
                column: "HotelServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRooms_RateRoomTypeId",
                table: "ReservationRooms",
                column: "RateRoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RateTimelines_RateRoomTypeId",
                table: "RateTimelines",
                column: "RateRoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RateRoomTypes_RateId",
                table: "RateRoomTypes",
                column: "RateId");

            migrationBuilder.AddForeignKey(
                name: "FK_RateTimelines_RateRoomTypes_RateRoomTypeId",
                table: "RateTimelines",
                column: "RateRoomTypeId",
                principalTable: "RateRoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationRooms_RateRoomTypes_RateRoomTypeId",
                table: "ReservationRooms",
                column: "RateRoomTypeId",
                principalTable: "RateRoomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
