using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookingEvent.Migrations
{
    /// <inheritdoc />
    public partial class updateorg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address1",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkedIn",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Youtube",
                table: "Organisation",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address1",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Address2",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "LinkedIn",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Organisation");

            migrationBuilder.DropColumn(
                name: "Youtube",
                table: "Organisation");
        }
    }
}
