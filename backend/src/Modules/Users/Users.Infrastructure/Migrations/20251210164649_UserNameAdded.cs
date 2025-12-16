using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UserNameAdded : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_tbl_User_Menus_tbl_User_Menus_Parent_Id",
            table: "tbl_User_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_User_Role_Menus_tbl_User_Menus_Menu_Id",
            table: "tbl_User_Role_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_User_Role_Menus_tbl_User_Roles_Role_Id",
            table: "tbl_User_Role_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_User_User_Roles_tbl_User_Roles_Role_Id",
            table: "tbl_User_User_Roles");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_User_User_Roles_tbl_User_Users_User_Id",
            table: "tbl_User_User_Roles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_User_Users",
            table: "tbl_User_Users");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_User_User_Roles",
            table: "tbl_User_User_Roles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_User_Roles",
            table: "tbl_User_Roles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_User_Role_Menus",
            table: "tbl_User_Role_Menus");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_User_Menus",
            table: "tbl_User_Menus");

        migrationBuilder.RenameTable(
            name: "tbl_User_Users",
            newName: "tbl_Users_Users");

        migrationBuilder.RenameTable(
            name: "tbl_User_User_Roles",
            newName: "tbl_Users_User_Roles");

        migrationBuilder.RenameTable(
            name: "tbl_User_Roles",
            newName: "tbl_Users_Roles");

        migrationBuilder.RenameTable(
            name: "tbl_User_Role_Menus",
            newName: "tbl_Users_Role_Menus");

        migrationBuilder.RenameTable(
            name: "tbl_User_Menus",
            newName: "tbl_Users_Menus");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_User_User_Roles_User_Id",
            table: "tbl_Users_User_Roles",
            newName: "IX_tbl_Users_User_Roles_User_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_User_User_Roles_Role_Id",
            table: "tbl_Users_User_Roles",
            newName: "IX_tbl_Users_User_Roles_Role_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_User_Role_Menus_Role_Id",
            table: "tbl_Users_Role_Menus",
            newName: "IX_tbl_Users_Role_Menus_Role_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_User_Role_Menus_Menu_Id",
            table: "tbl_Users_Role_Menus",
            newName: "IX_tbl_Users_Role_Menus_Menu_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_User_Menus_Parent_Id",
            table: "tbl_Users_Menus",
            newName: "IX_tbl_Users_Menus_Parent_Id");

        migrationBuilder.AddColumn<string>(
            name: "User_Name",
            table: "tbl_Users_Users",
            type: "longtext",
            nullable: false)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_Users",
            table: "tbl_Users_Users",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_User_Roles",
            table: "tbl_Users_User_Roles",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_Roles",
            table: "tbl_Users_Roles",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_Role_Menus",
            table: "tbl_Users_Role_Menus",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_Users_Menus",
            table: "tbl_Users_Menus",
            column: "Id");

        migrationBuilder.UpdateData(
            table: "tbl_Users_Users",
            keyColumn: "Id",
            keyValue: 1,
            column: "User_Name",
            value: "Admin");

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_Menus_tbl_Users_Menus_Parent_Id",
            table: "tbl_Users_Menus",
            column: "Parent_Id",
            principalTable: "tbl_Users_Menus",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

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
            name: "FK_tbl_Users_User_Roles_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_User_Roles",
            column: "Role_Id",
            principalTable: "tbl_Users_Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Users_User_Roles_tbl_Users_Users_User_Id",
            table: "tbl_Users_User_Roles",
            column: "User_Id",
            principalTable: "tbl_Users_Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_Menus_tbl_Users_Menus_Parent_Id",
            table: "tbl_Users_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_Role_Menus_tbl_Users_Menus_Menu_Id",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_Role_Menus_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_User_Roles_tbl_Users_Roles_Role_Id",
            table: "tbl_Users_User_Roles");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Users_User_Roles_tbl_Users_Users_User_Id",
            table: "tbl_Users_User_Roles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_Users",
            table: "tbl_Users_Users");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_User_Roles",
            table: "tbl_Users_User_Roles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_Roles",
            table: "tbl_Users_Roles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_Role_Menus",
            table: "tbl_Users_Role_Menus");

        migrationBuilder.DropPrimaryKey(
            name: "PK_tbl_Users_Menus",
            table: "tbl_Users_Menus");

        migrationBuilder.DropColumn(
            name: "User_Name",
            table: "tbl_Users_Users");

        migrationBuilder.RenameTable(
            name: "tbl_Users_Users",
            newName: "tbl_User_Users");

        migrationBuilder.RenameTable(
            name: "tbl_Users_User_Roles",
            newName: "tbl_User_User_Roles");

        migrationBuilder.RenameTable(
            name: "tbl_Users_Roles",
            newName: "tbl_User_Roles");

        migrationBuilder.RenameTable(
            name: "tbl_Users_Role_Menus",
            newName: "tbl_User_Role_Menus");

        migrationBuilder.RenameTable(
            name: "tbl_Users_Menus",
            newName: "tbl_User_Menus");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_User_Roles_User_Id",
            table: "tbl_User_User_Roles",
            newName: "IX_tbl_User_User_Roles_User_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_User_Roles_Role_Id",
            table: "tbl_User_User_Roles",
            newName: "IX_tbl_User_User_Roles_Role_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_Role_Menus_Role_Id",
            table: "tbl_User_Role_Menus",
            newName: "IX_tbl_User_Role_Menus_Role_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_Role_Menus_Menu_Id",
            table: "tbl_User_Role_Menus",
            newName: "IX_tbl_User_Role_Menus_Menu_Id");

        migrationBuilder.RenameIndex(
            name: "IX_tbl_Users_Menus_Parent_Id",
            table: "tbl_User_Menus",
            newName: "IX_tbl_User_Menus_Parent_Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_User_Users",
            table: "tbl_User_Users",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_User_User_Roles",
            table: "tbl_User_User_Roles",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_User_Roles",
            table: "tbl_User_Roles",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_User_Role_Menus",
            table: "tbl_User_Role_Menus",
            column: "Id");

        migrationBuilder.AddPrimaryKey(
            name: "PK_tbl_User_Menus",
            table: "tbl_User_Menus",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_User_Menus_tbl_User_Menus_Parent_Id",
            table: "tbl_User_Menus",
            column: "Parent_Id",
            principalTable: "tbl_User_Menus",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_User_Role_Menus_tbl_User_Menus_Menu_Id",
            table: "tbl_User_Role_Menus",
            column: "Menu_Id",
            principalTable: "tbl_User_Menus",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_User_Role_Menus_tbl_User_Roles_Role_Id",
            table: "tbl_User_Role_Menus",
            column: "Role_Id",
            principalTable: "tbl_User_Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_User_User_Roles_tbl_User_Roles_Role_Id",
            table: "tbl_User_User_Roles",
            column: "Role_Id",
            principalTable: "tbl_User_Roles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_User_User_Roles_tbl_User_Users_User_Id",
            table: "tbl_User_User_Roles",
            column: "User_Id",
            principalTable: "tbl_User_Users",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
