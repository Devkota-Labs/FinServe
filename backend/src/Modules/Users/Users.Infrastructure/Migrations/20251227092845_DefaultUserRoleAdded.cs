using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefaultUserRoleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbl_Users_RoleMenus",
                columns: ["Id", "Created_By", "Created_Time", "Last_Updated_By", "Last_Updated_Time", "Menu_Id", "Role_Id"],
                values: [1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 1]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: ["Description", "Name"],
                values: ["Default Role", "Default"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: ["Description", "Name"],
                values: ["Platform administrator", "Admin"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: ["Description", "Name"],
                values: ["Internal employee", "Employee"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: ["Description", "Name"],
                values: ["Car dealer", "Dealer"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: ["Description", "Name"],
                values: ["Bank representative", "Banker"]);

            migrationBuilder.InsertData(
                table: "tbl_Users_Roles",
                columns: ["Id", "Created_By", "Created_Time", "Description", "Is_Active", "Last_Updated_By", "Last_Updated_Time", "Name"],
                values: [6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "End customer", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role_Id",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_Users_RoleMenus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: ["Description", "Name"],
                values: ["Platform administrator", "Admin"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: ["Description", "Name"],
                values: ["Internal employee", "Employee"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: ["Description", "Name"],
                values: ["Car dealer", "Dealer"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: ["Description", "Name"],
                values: ["Bank representative", "Banker"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_Roles",
                keyColumn: "Id",
                keyValue: 5,
                columns: ["Description", "Name"],
                values: ["End customer", "Customer"]);

            migrationBuilder.UpdateData(
                table: "tbl_Users_UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Role_Id",
                value: 1);
        }
    }
}
