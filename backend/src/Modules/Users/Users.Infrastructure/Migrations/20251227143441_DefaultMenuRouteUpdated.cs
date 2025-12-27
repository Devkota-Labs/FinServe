using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefaultMenuRouteUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Users_Menus",
                keyColumn: "Id",
                keyValue: 9,
                column: "Route",
                value: "/profile");

            migrationBuilder.UpdateData(
                table: "tbl_Users_Menus",
                keyColumn: "Id",
                keyValue: 10,
                column: "Route",
                value: "/auth/change-password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Users_Menus",
                keyColumn: "Id",
                keyValue: 9,
                column: "Route",
                value: "/admin/dashboard/masters/menus");

            migrationBuilder.UpdateData(
                table: "tbl_Users_Menus",
                keyColumn: "Id",
                keyValue: 10,
                column: "Route",
                value: "/admin/dashboard/masters/menus");
        }
    }
}
