using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminUserRoleSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Users_User_Roles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_User_Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Users_User_Roles_tbl_Users_Users_User_Id",
                table: "tbl_Users_User_Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Users_User_Roles",
                table: "tbl_Users_User_Roles");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Users_User_Roles_User_Id",
                table: "tbl_Users_User_Roles");

            migrationBuilder.DeleteData(
                table: "tbl_Users_Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "tbl_Users_User_Roles",
                newName: "tbl_Users_UserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Users_User_Roles_Role_Id",
                table: "tbl_Users_UserRoles",
                newName: "IX_tbl_Users_UserRoles_Role_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Users_UserRoles",
                table: "tbl_Users_UserRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Users_UserRoles_User_Id_Role_Id",
                table: "tbl_Users_UserRoles",
                columns: new[] { "User_Id", "Role_Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_UserRoles",
                column: "Role_Id",
                principalTable: "tbl_Users_Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Users_User_Id",
                table: "tbl_Users_UserRoles",
                column: "User_Id",
                principalTable: "tbl_Users_Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Roles_Role_Id",
                table: "tbl_Users_UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_tbl_Users_UserRoles_tbl_Users_Users_User_Id",
                table: "tbl_Users_UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Users_UserRoles",
                table: "tbl_Users_UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_tbl_Users_UserRoles_User_Id_Role_Id",
                table: "tbl_Users_UserRoles");

            migrationBuilder.RenameTable(
                name: "tbl_Users_UserRoles",
                newName: "tbl_Users_User_Roles");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Users_UserRoles_Role_Id",
                table: "tbl_Users_User_Roles",
                newName: "IX_tbl_Users_User_Roles_Role_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Users_User_Roles",
                table: "tbl_Users_User_Roles",
                column: "Id");

            migrationBuilder.InsertData(
                table: "tbl_Users_Users",
                columns: new[] { "Id", "Address", "City_Id", "Country_Id", "Created_By", "Created_Time", "Date_Of_Birth", "Device_Tokens_Json", "Email", "Email_Verified", "Failed_Login_Count", "First_Name", "Gender", "Is_Active", "Is_Approved", "Last_Name", "Last_Updated_By", "Last_Updated_Time", "Lockout_End_At", "Mfa_Enabled", "Mfa_Secret", "Middle_Name", "Mobile", "Mobile_Verified", "Password_Expiry_Date", "Password_Hash", "Password_Last_Changed", "Pin_Code", "Profile_Image_Url", "State_Id", "User_Name" },
                values: new object[] { 1, "123 Admin St, Metropolis", 1, 1, "Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 1, 1), null, "admin@finserve.com", true, 0, "System", 3, true, true, "Administrator", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "9999999999", true, null, "AENIe4W4SbJh10PQDlXCrz7vyJmLQulPRIuFhXqE+p41Pf0DRGhLa+CDx6EkNjHfhg==", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400001", null, 1, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Users_User_Roles_User_Id",
                table: "tbl_Users_User_Roles",
                column: "User_Id");

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
    }
}
