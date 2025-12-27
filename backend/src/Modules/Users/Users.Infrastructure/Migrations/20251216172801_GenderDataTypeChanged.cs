using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class GenderDataTypeChanged : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Gender",
            table: "tbl_Users_Users",
            type: "varchar(50)",
            maxLength: 50,
            nullable: false,
            oldClrType: typeof(int),
            oldType: "int")
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "tbl_Users_Users",
            keyColumn: "Id",
            keyValue: 1,
            column: "Gender",
            value: "PerferNotToSay");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "Gender",
            table: "tbl_Users_Users",
            type: "int",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "varchar(50)",
            oldMaxLength: 50)
            .OldAnnotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.UpdateData(
            table: "tbl_Users_Users",
            keyColumn: "Id",
            keyValue: 1,
            column: "Gender",
            value: 3);
    }
}
