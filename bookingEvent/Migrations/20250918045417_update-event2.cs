using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookingEvent.Migrations
{
    /// <inheritdoc />
    public partial class updateevent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Organisation_OrganisationId",
                table: "Event");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "Event",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Organisation_OrganisationId",
                table: "Event",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_Organisation_OrganisationId",
                table: "Event");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "Event",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_Organisation_OrganisationId",
                table: "Event",
                column: "OrganisationId",
                principalTable: "Organisation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
