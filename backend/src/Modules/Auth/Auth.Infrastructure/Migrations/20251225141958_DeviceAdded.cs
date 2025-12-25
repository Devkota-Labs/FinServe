using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations;

/// <inheritdoc />
public partial class DeviceAdded : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "User_Agent",
            table: "tbl_Auth_LoginHistories",
            type: "longtext",
            nullable: false)
            .Annotation("MySql:CharSet", "utf8mb4");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "User_Agent",
            table: "tbl_Auth_LoginHistories");
    }
}
