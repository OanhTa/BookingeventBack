using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bookingEvent.Migrations
{
    /// <inheritdoc />
    public partial class editrolepermistion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGranted",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGranted",
                table: "RolePermissions");
        }
    }
}
