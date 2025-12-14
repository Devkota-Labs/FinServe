using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLogoutTimeToLoginHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.AlterColumn<int>(
                name: "User_Id",
                table: "tbl_Auth_LoginHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Login_Time",
                table: "tbl_Auth_LoginHistories",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Failure_Reason",
                table: "tbl_Auth_LoginHistories",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "Is_Success",
                table: "tbl_Auth_LoginHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Session_Id",
                table: "tbl_Auth_LoginHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Auth_LoginHistories_Session_Id",
                table: "tbl_Auth_LoginHistories",
                column: "Session_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Auth_LoginHistories_User_Id_Login_Time",
                table: "tbl_Auth_LoginHistories",
                columns: new[] { "User_Id", "Login_Time" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_Auth_LoginHistories_Session_Id",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Auth_LoginHistories_User_Id_Login_Time",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.DropColumn(
                name: "Failure_Reason",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.DropColumn(
                name: "Is_Success",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.DropColumn(
                name: "Session_Id",
                table: "tbl_Auth_LoginHistories");

            migrationBuilder.AlterColumn<int>(
                name: "User_Id",
                table: "tbl_Auth_LoginHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Login_Time",
                table: "tbl_Auth_LoginHistories",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "tbl_Auth_LoginHistories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "tbl_Auth_LoginHistories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
