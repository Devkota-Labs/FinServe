using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class RoleMenuTableNameChanged : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_Role_Menus_tbl_Users_Menus_Menu_Id",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_Role_Menus_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_UserRoles_tbl_Users_Users_User_Id",
            table: "tbl_Users_UserRoles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_Role_Menus",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.DropIndex(
            name: "IX_tbl_Users_Role_Menus_Role_Id",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.RenameTable(
            name: "tbl_Users_Role_Menus",
            newName: "tbl_Users_RoleMenus");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_Role_Menus_Menu_Id",
            table: "tbl_Users_RoleMenus",
            newName: "IX_tbl_Users_RoleMenus_Menu_Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_RoleMenus",
            table: "tbl_Users_RoleMenus",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Users_RoleMenus_Role_Id_Menu_Id",
            table: "tbl_Users_RoleMenus",
            columns: new[] { "Role_Id", "Menu_Id" },
            unique: true);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_RoleMenus_tbl_Users_Menus_Menu_Id",
            table: "tbl_Users_RoleMenus",
            column: "Menu_Id",
            principalTable: "tbl_Users_Menus",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_RoleMenus_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_RoleMenus",
            column: "Role_Id",
            principalTable: "tbl_Users_Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_UserRoles_tbl_Users_Users_User_Id",
            table: "tbl_Users_UserRoles",
            column: "User_Id",
            principalTable: "tbl_Users_Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_RoleMenus_tbl_Users_Menus_Menu_Id",
            table: "tbl_Users_RoleMenus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_RoleMenus_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_RoleMenus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_UserRoles_tbl_Users_Users_User_Id",
            table: "tbl_Users_UserRoles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_RoleMenus",
            table: "tbl_Users_RoleMenus");

        migrationBuilder.DropIndex(
            name: "IX_tbl_Users_RoleMenus_Role_Id_Menu_Id",
            table: "tbl_Users_RoleMenus");

        migrationBuilder.RenameTable(
            name: "tbl_Users_RoleMenus",
            newName: "tbl_Users_Role_Menus");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_RoleMenus_Menu_Id",
            table: "tbl_Users_Role_Menus",
            newName: "IX_tbl_Users_Role_Menus_Menu_Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_Role_Menus",
            table: "tbl_Users_Role_Menus",
            column: "Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Users_Role_Menus_Role_Id",
            table: "tbl_Users_Role_Menus",
            column: "Role_Id");

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_Role_Menus_tbl_Users_Menus_Menu_Id",
            table: "tbl_Users_Role_Menus",
            column: "Menu_Id",
            principalTable: "tbl_Users_Menus",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_Role_Menus_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_Role_Menus",
            column: "Role_Id",
            principalTable: "tbl_Users_Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_UserRoles_tbl_Users_Users_User_Id",
            table: "tbl_Users_UserRoles",
            column: "User_Id",
            principalTable: "tbl_Users_Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
