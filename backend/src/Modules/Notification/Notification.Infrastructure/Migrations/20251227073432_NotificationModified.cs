using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notification.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NotificationModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Notification_NotificationOutbox",
                table: "tbl_Notification_NotificationOutbox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Notification_NotificationDeduplications",
                table: "tbl_Notification_NotificationDeduplications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Notification_InAppNotifications",
                table: "tbl_Notification_InAppNotifications");

            migrationBuilder.RenameTable(
                name: "tbl_Notification_NotificationOutbox",
                newName: "tbl_Notifications_Outbox");

            migrationBuilder.RenameTable(
                name: "tbl_Notification_NotificationDeduplications",
                newName: "tbl_Notifications_Deduplications");

            migrationBuilder.RenameTable(
                name: "tbl_Notification_InAppNotifications",
                newName: "tbl_Notifications_InAppNotifications");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Notification_NotificationOutbox_Status",
                table: "tbl_Notifications_Outbox",
                newName: "IX_tbl_Notifications_Outbox_Status");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Notification_NotificationDeduplications_User_Id_Type_Cha~",
                table: "tbl_Notifications_Deduplications",
                newName: "IX_tbl_Notifications_Deduplications_User_Id_Type_Channel_Dedup_~");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Notification_InAppNotifications_User_Id_Is_Read",
                table: "tbl_Notifications_InAppNotifications",
                newName: "IX_tbl_Notifications_InAppNotifications_User_Id_Is_Read");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Notifications_Outbox",
                table: "tbl_Notifications_Outbox",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Notifications_Deduplications",
                table: "tbl_Notifications_Deduplications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Notifications_InAppNotifications",
                table: "tbl_Notifications_InAppNotifications",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Notifications_Outbox",
                table: "tbl_Notifications_Outbox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Notifications_InAppNotifications",
                table: "tbl_Notifications_InAppNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_Notifications_Deduplications",
                table: "tbl_Notifications_Deduplications");

            migrationBuilder.RenameTable(
                name: "tbl_Notifications_Outbox",
                newName: "tbl_Notification_NotificationOutbox");

            migrationBuilder.RenameTable(
                name: "tbl_Notifications_InAppNotifications",
                newName: "tbl_Notification_InAppNotifications");

            migrationBuilder.RenameTable(
                name: "tbl_Notifications_Deduplications",
                newName: "tbl_Notification_NotificationDeduplications");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Notifications_Outbox_Status",
                table: "tbl_Notification_NotificationOutbox",
                newName: "IX_tbl_Notification_NotificationOutbox_Status");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Notifications_InAppNotifications_User_Id_Is_Read",
                table: "tbl_Notification_InAppNotifications",
                newName: "IX_tbl_Notification_InAppNotifications_User_Id_Is_Read");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_Notifications_Deduplications_User_Id_Type_Channel_Dedup_~",
                table: "tbl_Notification_NotificationDeduplications",
                newName: "IX_tbl_Notification_NotificationDeduplications_User_Id_Type_Cha~");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Notification_NotificationOutbox",
                table: "tbl_Notification_NotificationOutbox",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Notification_InAppNotifications",
                table: "tbl_Notification_InAppNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_Notification_NotificationDeduplications",
                table: "tbl_Notification_NotificationDeduplications",
                column: "Id");
        }
    }
}
