using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AdminDefaultPasswordChanged : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "tbl_Users_Users",
            keyColumn: "Id",
            keyValue: 1,
            column: "Password_Hash",
            value: "AENIe4W4SbJh10PQDlXCrz7vyJmLQulPRIuFhXqE+p41Pf0DRGhLa+CDx6EkNjHfhg==");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.UpdateData(
            table: "tbl_Users_Users",
            keyColumn: "Id",
            keyValue: 1,
            column: "Password_Hash",
            value: "AMjWYtPmGZURetbp6dI1r3XfmgZy0nrn7FU7te333XPDv3gqwQzRZNtcoka4Sow++Q==");
    }
}
