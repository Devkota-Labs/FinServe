using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UserAddressAdded : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Address",
            table: "tbl_Users_Users");

        migrationBuilder.DropColumn(
            name: "City_Id",
            table: "tbl_Users_Users");

        migrationBuilder.DropColumn(
            name: "Country_Id",
            table: "tbl_Users_Users");

        migrationBuilder.DropColumn(
            name: "Pin_Code",
            table: "tbl_Users_Users");

        migrationBuilder.DropColumn(
            name: "State_Id",
            table: "tbl_Users_Users");

        migrationBuilder.CreateTable(
            name: "tbl_Users_UserAddresses",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Address_Type = table.Column<int>(type: "int", nullable: false),
                Address_Line1 = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Address_Line2 = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Country_Id = table.Column<int>(type: "int", nullable: false),
                State_Id = table.Column<int>(type: "int", nullable: false),
                City_Id = table.Column<int>(type: "int", nullable: false),
                Pin_Code = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Is_Primary = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Users_UserAddresses", x => x.Id);
                table.ForeignKey(
                    name: "FK_tbl_Users_UserAddresses_tbl_Users_Users_User_Id",
                    column: x => x.User_Id,
                    principalTable: "tbl_Users_Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            table: "tbl_Users_UserAddresses",
            columns: ["Id", "Address_Line1", "Address_Line2", "Address_Type", "City_Id", "Country_Id", "Created_By", "Created_Time", "Is_Primary", "Last_Updated_By", "Last_Updated_Time", "Pin_Code", "State_Id", "User_Id"],
            values: [1, "123 Admin St", null, 1, 1, 1, "Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400001", 1, 1]);

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Users_UserAddresses_User_Id",
            table: "tbl_Users_UserAddresses",
            column: "User_Id",
            unique: true,
            filter: "[IsPrimary] = 1");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Users_UserAddresses_User_Id_Is_Primary",
            table: "tbl_Users_UserAddresses",
            columns: ["User_Id", "Is_Primary"]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tbl_Users_UserAddresses");

        migrationBuilder.AddColumn<string>(
            name: "Address",
            table: "tbl_Users_Users",
            type: "longtext",
            nullable: false)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddColumn<int>(
            name: "City_Id",
            table: "tbl_Users_Users",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<int>(
            name: "Country_Id",
            table: "tbl_Users_Users",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.AddColumn<string>(
            name: "Pin_Code",
            table: "tbl_Users_Users",
            type: "longtext",
            nullable: false)
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.AddColumn<int>(
            name: "State_Id",
            table: "tbl_Users_Users",
            type: "int",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.UpdateData(
            table: "tbl_Users_Users",
            keyColumn: "Id",
            keyValue: 1,
            columns: ["Address", "City_Id", "Country_Id", "Pin_Code", "State_Id"],
            values: ["123 Admin St, Metropolis", 1, 1, "400001", 1]);
    }
}
