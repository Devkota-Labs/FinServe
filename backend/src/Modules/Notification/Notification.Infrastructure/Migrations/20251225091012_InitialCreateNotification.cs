using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreateNotification : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "tbl_Notification_UserNotifications",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                User_Id = table.Column<int>(type: "int", nullable: false),
                Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Message = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                    .Annotation("MySql:CharSet", "utf8mb4"),
                Category = table.Column<int>(type: "int", nullable: false),
                Severity = table.Column<int>(type: "int", nullable: false),
                Action_Type = table.Column<int>(type: "int", nullable: false),
                Reference_Type = table.Column<int>(type: "int", nullable: false),
                Reference_Id = table.Column<int>(type: "int", nullable: true),
                Is_Read = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                Created_At = table.Column<DateTime>(type: "datetime(6)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tbl_Notification_UserNotifications", x => x.Id);
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateIndex(
            name: "IX_tbl_Notification_UserNotifications_User_Id_Is_Read",
            table: "tbl_Notification_UserNotifications",
            columns: ["User_Id", "Is_Read"]);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tbl_Notification_UserNotifications");
    }
}
