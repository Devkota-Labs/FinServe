using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Location.Infrastructure.Migrations;

/// <inheritdoc />
public partial class Location_Initial_Create : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        ArgumentNullException.ThrowIfNull(migrationBuilder);

        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Location_Countries",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Iso_Code = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Mobile_Code = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Location_Countries", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Location_States",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Country_Id = table.Column<int>(type: "int", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Location_States", x => x.Id);
                table.ForeignKey(
                    name: "FK_tbl_Location_States_tbl_Location_Countries_Country_Id",
                    column: x => x.Country_Id,
                    principalTable: "tbl_Location_Countries",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Location_Cities",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                State_Id = table.Column<int>(type: "int", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Location_Cities", x => x.Id);
                table.ForeignKey(
                    name: "FK_tbl_Location_Cities_tbl_Location_States_State_Id",
                    column: x => x.State_Id,
                    principalTable: "tbl_Location_States",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            table: "tbl_Location_Countries",
            columns: ["Id", "Created_By", "Created_Time", "Iso_Code", "Last_Updated_By", "Last_Updated_Time", "Mobile_Code", "Name"],
            values: new object[,]
            {
                { 1, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AF", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+93", "Afghanistan" },
                { 2, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AL", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+355", "Albania" },
                { 3, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DZ", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+213", "Algeria" },
                { 4, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AD", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+376", "Andorra" },
                { 5, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AO", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+244", "Angola" },
                { 6, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AG", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1268", "Antigua and Barbuda" },
                { 7, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AR", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+54", "Argentina" },
                { 8, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AM", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+374", "Armenia" },
                { 9, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AU", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+61", "Australia" },
                { 10, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AT", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+43", "Austria" },
                { 11, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AZ", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+994", "Azerbaijan" },
                { 12, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BS", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1242", "Bahamas" },
                { 13, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BH", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+973", "Bahrain" },
                { 14, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BD", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+880", "Bangladesh" },
                { 15, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BB", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1246", "Barbados" },
                { 16, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BY", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+375", "Belarus" },
                { 17, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BE", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+32", "Belgium" },
                { 18, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BZ", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+501", "Belize" },
                { 19, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BJ", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+229", "Benin" },
                { 20, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BT", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+975", "Bhutan" },
                { 21, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BO", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+591", "Bolivia" },
                { 22, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BA", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+387", "Bosnia and Herzegovina" },
                { 23, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BW", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+267", "Botswana" },
                { 24, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BR", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+55", "Brazil" },
                { 25, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BN", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+673", "Brunei Darussalam" },
                { 75, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IN", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+91", "India" },
                { 183, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "GB", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+44", "United Kingdom" },
                { 184, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "US", "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "+1", "United States of America" }
            });

        migrationBuilder.InsertData(
            table: "tbl_Location_States",
            columns: ["Id", "Country_Id", "Created_By", "Created_Time", "Last_Updated_By", "Last_Updated_Time", "Name"],
            values: new object[,]
            {
                { 1, 75, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maharashtra" },
                { 2, 75, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Karnataka" },
                { 3, 75, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tamil Nadu" }
            });

        migrationBuilder.InsertData(
            table: "tbl_Location_Cities",
            columns: ["Id", "Created_By", "Created_Time", "Last_Updated_By", "Last_Updated_Time", "Name", "State_Id"],
            values: new object[,]
            {
                { 1, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mumbai", 1 },
                { 2, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pune", 1 },
                { 3, "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Seeder", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bengaluru", 2 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Location_Cities_State_Id",
            table: "tbl_Location_Cities",
            column: "State_Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Location_States_Country_Id",
            table: "tbl_Location_States",
            column: "Country_Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        ArgumentNullException.ThrowIfNull(migrationBuilder);

        migrationBuilder.DropTable(
            name: "tbl_Location_Cities");

        migrationBuilder.DropTable(
            name: "tbl_Location_States");

        migrationBuilder.DropTable(
            name: "tbl_Location_Countries");
    }
}
