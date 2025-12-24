using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Location.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ForeignKeyRuleChanged : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Location_Cities_tbl_Location_States_State_Id",
            table: "tbl_Location_Cities");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Location_States_tbl_Location_Countries_Country_Id",
            table: "tbl_Location_States");

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Location_Cities_tbl_Location_States_State_Id",
            table: "tbl_Location_Cities",
            column: "State_Id",
            principalTable: "tbl_Location_States",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Location_States_tbl_Location_Countries_Country_Id",
            table: "tbl_Location_States",
            column: "Country_Id",
            principalTable: "tbl_Location_Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Location_Cities_tbl_Location_States_State_Id",
            table: "tbl_Location_Cities");

        migrationBuilder.DropForeignKey(
            name: "FK_tbl_Location_States_tbl_Location_Countries_Country_Id",
            table: "tbl_Location_States");

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Location_Cities_tbl_Location_States_State_Id",
            table: "tbl_Location_Cities",
            column: "State_Id",
            principalTable: "tbl_Location_States",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_tbl_Location_States_tbl_Location_Countries_Country_Id",
            table: "tbl_Location_States",
            column: "Country_Id",
            principalTable: "tbl_Location_Countries",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}
