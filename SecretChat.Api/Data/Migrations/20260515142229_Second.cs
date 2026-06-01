using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretChat.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavoriteForFirstUser",
                table: "Chats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFavoriteForSecondUser",
                table: "Chats",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavoriteForFirstUser",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "IsFavoriteForSecondUser",
                table: "Chats");
        }
    }
}
