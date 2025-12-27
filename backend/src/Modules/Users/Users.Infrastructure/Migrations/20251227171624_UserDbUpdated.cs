using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserDbUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "User_Name",
                table: "tbl_Users_Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Mobile",
                table: "tbl_Users_Users",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Users_Users_Email",
                table: "tbl_Users_Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Users_Users_Mobile",
                table: "tbl_Users_Users",
                column: "Mobile");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Users_Users_User_Name",
                table: "tbl_Users_Users",
                column: "User_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_Users_Users_Email",
                table: "tbl_Users_Users");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Users_Users_Mobile",
                table: "tbl_Users_Users");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Users_Users_User_Name",
                table: "tbl_Users_Users");

            migrationBuilder.AlterColumn<string>(
                name: "User_Name",
                table: "tbl_Users_Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Mobile",
                table: "tbl_Users_Users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
