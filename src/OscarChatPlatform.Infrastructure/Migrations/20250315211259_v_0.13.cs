using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OscarChatPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v_013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChat_AspNetUsers_UsersId",
                table: "ApplicationUserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChat_Chats_ChatsId",
                table: "ApplicationUserChat");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ApplicationUserChat",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ChatsId",
                table: "ApplicationUserChat",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserChat_UsersId",
                table: "ApplicationUserChat",
                newName: "IX_ApplicationUserChat_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChat_AspNetUsers_UserId",
                table: "ApplicationUserChat",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChat_Chats_ChatId",
                table: "ApplicationUserChat",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChat_AspNetUsers_UserId",
                table: "ApplicationUserChat");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserChat_Chats_ChatId",
                table: "ApplicationUserChat");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ApplicationUserChat",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "ApplicationUserChat",
                newName: "ChatsId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserChat_UserId",
                table: "ApplicationUserChat",
                newName: "IX_ApplicationUserChat_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChat_AspNetUsers_UsersId",
                table: "ApplicationUserChat",
                column: "UsersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserChat_Chats_ChatsId",
                table: "ApplicationUserChat",
                column: "ChatsId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
