using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OscarChatPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v_02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Chat",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroupChat",
                table: "Chats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ChatRoomQueues",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRoomQueues_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomQueues_UserId",
                table: "ChatRoomQueues",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "ChatRoomQueues");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsGroupChat",
                table: "Chats");

            migrationBuilder.AddColumn<string>(
                name: "Chat",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
