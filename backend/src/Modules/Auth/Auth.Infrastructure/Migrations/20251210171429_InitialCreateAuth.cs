using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auth.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreateAuth : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_EmailVerificationTokens",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                Email = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Token = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                ExpiryTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                IsUsed = table.Column<bool>(type: "tinyint(1)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_EmailVerificationTokens", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_LoginHistories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: true),
                Email = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Login_Time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                Logout_Time = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                Ip_Address = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Device = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Status = table.Column<int>(type: "int", nullable: false),
                Message = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_LoginHistories", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_MobileVerificationTokens",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Mobile_Number = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Token = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Expires_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Is_Used = table.Column<bool>(type: "tinyint(1)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_MobileVerificationTokens", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_Otp_Verifications",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Token = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Purpose = table.Column<int>(type: "int", nullable: false),
                Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Expires_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Consumed_At = table.Column<DateTime>(type: "datetime(6)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_Otp_Verifications", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_PasswordHistories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Password_Hash = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Created_Time = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_PasswordHistories", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_PasswordResetTokens",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Token = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Expires_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Used = table.Column<bool>(type: "tinyint(1)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_PasswordResetTokens", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Auth_RefreshTokens",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Token = table.Column<string>(type: "longtext", nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Is_Used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Expires_At = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                Created_By_Ip = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Revoked_At = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                Revoked_By_Ip = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Replaced_By_Token = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Reason_Revoked = table.Column<string>(type: "longtext", nullable: true)
                    .Annotation("MySql:CharSet", "utf8mb4")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Auth_RefreshTokens", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Auth_PasswordHistories_User_Id",
            table: "tbl_Auth_PasswordHistories",
            column: "User_Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tbl_Auth_EmailVerificationTokens");

        migrationBuilder.DropTable(
            name: "tbl_Auth_LoginHistories");

        migrationBuilder.DropTable(
            name: "tbl_Auth_MobileVerificationTokens");

        migrationBuilder.DropTable(
            name: "tbl_Auth_Otp_Verifications");

        migrationBuilder.DropTable(
            name: "tbl_Auth_PasswordHistories");

        migrationBuilder.DropTable(
            name: "tbl_Auth_PasswordResetTokens");

        migrationBuilder.DropTable(
            name: "tbl_Auth_RefreshTokens");
    }
}
