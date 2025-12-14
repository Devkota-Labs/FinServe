using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreateUser : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_User_Menus",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Route = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Icon = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Parent_Id = table.Column<int>(type: "int", nullable: true),
                Sequence = table.Column<int>(type: "int", nullable: false),
                Is_Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_User_Menus", x => x.Id);
                table.ForeignKey(
                    name: "FK_tbl_User_Menus_tbl_User_Menus_Parent_Id",
                    column: x => x.Parent_Id,
                    principalTable: "tbl_User_Menus",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_User_Roles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Description = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Is_Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_User_Roles", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_User_Users",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Mobile = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Gender = table.Column<int>(type: "int", nullable: false),
                Date_Of_Birth = table.Column<DateOnly>(type: "date", nullable: false),
                First_Name = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Middle_Name = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Name = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Country_Id = table.Column<int>(type: "int", nullable: false),
                City_Id = table.Column<int>(type: "int", nullable: false),
                State_Id = table.Column<int>(type: "int", nullable: false),
                Address = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Pin_Code = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Profile_Image_Url = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Is_Active = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Is_Approved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Email_Verified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Mobile_Verified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Password_Hash = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Password_Last_Changed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Password_Expiry_Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                Failed_Login_Count = table.Column<int>(type: "int", nullable: false),
                Lockout_End_At = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                Mfa_Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Mfa_Secret = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Device_Tokens_Json = table.Column<string>(type: "longtext", nullable: true)
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
                table.PrimaryKey("PK_tbl_User_Users", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_User_Role_Menus",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Role_Id = table.Column<int>(type: "int", nullable: false),
                Menu_Id = table.Column<int>(type: "int", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_User_Role_Menus", x => x.Id);
                table.ForeignKey(
                    name: "FK_tbl_User_Role_Menus_tbl_User_Menus_Menu_Id",
                    column: x => x.Menu_Id,
                    principalTable: "tbl_User_Menus",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_User_Role_Menus_tbl_User_Roles_Role_Id",
                    column: x => x.Role_Id,
                    principalTable: "tbl_User_Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_User_User_Roles",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Role_Id = table.Column<int>(type: "int", nullable: false),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Last_Updated_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Last_Updated_By = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_User_User_Roles", x => x.Id);
                table.ForeignKey(
                    name: "FK_tbl_User_User_Roles_tbl_User_Roles_Role_Id",
                    column: x => x.Role_Id,
                    principalTable: "tbl_User_Roles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_tbl_User_User_Roles_tbl_User_Users_User_Id",
                    column: x => x.User_Id,
                    principalTable: "tbl_User_Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            table: "tbl_User_Menus",
            columns: new[] { "Id", "Created_By", "Created_Time", "Icon", "Is_Active", "Last_Updated_By", "Last_Updated_Time", "Name", "Parent_Id", "Route", "Sequence" },
            values: new object[,]
            {
                { 1, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Profile", null, "#", 1 },
                { 2, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Masters", null, "#", 2 },
                { 3, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Users", null, "#", 3 }
            });

        migrationBuilder.InsertData(
            table: "tbl_User_Roles",
            columns: new[] { "Id", "Created_By", "Created_Time", "Description", "Is_Active", "Last_Updated_By", "Last_Updated_Time", "Name" },
            values: new object[,]
            {
                { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Platform administrator", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin" },
                { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Internal employee", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Employee" },
                { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Car dealer", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dealer" },
                { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bank representative", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Banker" },
                { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "End customer", true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer" }
            });

        migrationBuilder.InsertData(
            table: "tbl_User_Users",
            columns: new[] { "Id", "Address", "City_Id", "Country_Id", "Created_By", "Created_Time", "Date_Of_Birth", "Device_Tokens_Json", "Email", "Email_Verified", "Failed_Login_Count", "First_Name", "Gender", "Is_Active", "Is_Approved", "Last_Name", "Last_Updated_By", "Last_Updated_Time", "Lockout_End_At", "Mfa_Enabled", "Mfa_Secret", "Middle_Name", "Mobile", "Mobile_Verified", "Password_Expiry_Date", "Password_Hash", "Password_Last_Changed", "Pin_Code", "Profile_Image_Url", "State_Id" },
            values: new object[] { 1, "123 Admin St, Metropolis", 1, 1, "Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 1, 1), null, "admin@finserve.com", true, 0, "System", 3, true, true, "Administrator", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "9999999999", true, null, "AMjWYtPmGZURetbp6dI1r3XfmgZy0nrn7FU7te333XPDv3gqwQzRZNtcoka4Sow++Q==", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400001", null, 1 });

        migrationBuilder.InsertData(
            table: "tbl_User_Menus",
            columns: new[] { "Id", "Created_By", "Created_Time", "Icon", "Is_Active", "Last_Updated_By", "Last_Updated_Time", "Name", "Parent_Id", "Route", "Sequence" },
            values: new object[,]
            {
                { 4, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Countries", 2, "/admin/masters/countries", 1 },
                { 5, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "States", 2, "/admin/masters/states", 2 },
                { 6, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cities", 2, "/admin/masters/cities", 3 },
                { 7, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Roles", 2, "/admin/masters/roles", 4 },
                { 8, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Menus", 2, "/admin/masters/menus", 5 },
                { 9, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "View Profile", 1, "/admin/dashboard/masters/menus", 1 },
                { 10, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Change Password", 1, "/admin/dashboard/masters/menus", 2 },
                { 11, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Users", 3, "/admin/user-management/all-users", 1 },
                { 12, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approve Users", 3, "/admin/user-management/approve-user", 2 },
                { 13, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Unlock Users", 3, "/admin/user-management/unlock-user", 3 },
                { 14, "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Assign Roles", 3, "/admin/user-management/assign-roles", 4 }
            });

        migrationBuilder.CreateIndex(
            name: "IX_tbl_User_Menus_Parent_Id",
            table: "tbl_User_Menus",
            column: "Parent_Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_User_Role_Menus_Menu_Id",
            table: "tbl_User_Role_Menus",
            column: "Menu_Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_User_Role_Menus_Role_Id",
            table: "tbl_User_Role_Menus",
            column: "Role_Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_User_User_Roles_Role_Id",
            table: "tbl_User_User_Roles",
            column: "Role_Id");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_User_User_Roles_User_Id",
            table: "tbl_User_User_Roles",
            column: "User_Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tbl_User_Role_Menus");

        migrationBuilder.DropTable(
            name: "tbl_User_User_Roles");

        migrationBuilder.DropTable(
            name: "tbl_User_Menus");

        migrationBuilder.DropTable(
            name: "tbl_User_Roles");

        migrationBuilder.DropTable(
            name: "tbl_User_Users");
    }
}
