using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SecretChat.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedWasReadedToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WasReaded",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WasReaded",
                table: "Messages");
        }
    }
}
