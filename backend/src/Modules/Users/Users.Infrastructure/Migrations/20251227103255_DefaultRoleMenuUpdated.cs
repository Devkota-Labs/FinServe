using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefaultRoleMenuUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "tbl_Users_RoleMenus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Menu_Id",
                value: 1);

            migrationBuilder.InsertData(
                table: "tbl_Users_RoleMenus",
                columns: ["Id", "Created_By", "Created_Time", "Last_Updated_By", "Last_Updated_Time", "Menu_Id", "Role_Id"],
                values: [2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_Users_RoleMenus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "tbl_Users_RoleMenus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Menu_Id",
                value: 9);
        }
    }
}
