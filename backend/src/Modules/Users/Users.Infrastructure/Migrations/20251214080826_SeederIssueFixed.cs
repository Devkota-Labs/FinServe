using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeederIssueFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tbl_Users_Users",
                columns: new[] { "Id", "Address", "City_Id", "Country_Id", "Created_By", "Created_Time", "Date_Of_Birth", "Device_Tokens_Json", "Email", "Email_Verified", "Failed_Login_Count", "First_Name", "Gender", "Is_Active", "Is_Approved", "Last_Name", "Last_Updated_By", "Last_Updated_Time", "Lockout_End_At", "Mfa_Enabled", "Mfa_Secret", "Middle_Name", "Mobile", "Mobile_Verified", "Password_Expiry_Date", "Password_Hash", "Password_Last_Changed", "Pin_Code", "Profile_Image_Url", "State_Id", "User_Name" },
                values: new object[] { 1, "123 Admin St, Metropolis", 1, 1, "Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateOnly(2024, 1, 1), null, "admin@finserve.com", true, 0, "System", 3, true, true, "Administrator", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, null, null, "9999999999", true, null, "AENIe4W4SbJh10PQDlXCrz7vyJmLQulPRIuFhXqE+p41Pf0DRGhLa+CDx6EkNjHfhg==", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "400001", null, 1, "Admin" });

            migrationBuilder.InsertData(
                table: "tbl_Users_UserRoles",
                columns: new[] { "Id", "Created_By", "Created_Time", "Last_Updated_By", "Last_Updated_Time", "Role_Id", "User_Id" },
                values: new object[] { 1, "Admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_Users_UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "tbl_Users_Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
