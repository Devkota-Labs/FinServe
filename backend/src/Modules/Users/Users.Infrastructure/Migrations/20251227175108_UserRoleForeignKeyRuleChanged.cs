using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserRoleForeignKeyRuleChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_UserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_UserRoles",
                column: "Role_Id",
                principalTable: "tbl_Users_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_UserRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_UserRoles",
                column: "Role_Id",
                principalTable: "tbl_Users_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
