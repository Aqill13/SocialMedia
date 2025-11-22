using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class UserSocialMediaTableUserIdPlatform_IsUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSocialLink_UserId",
                table: "UserSocialLink");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialLink_UserId_Platform",
                table: "UserSocialLink",
                columns: new[] { "UserId", "Platform" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSocialLink_UserId_Platform",
                table: "UserSocialLink");

            migrationBuilder.CreateIndex(
                name: "IX_UserSocialLink_UserId",
                table: "UserSocialLink",
                column: "UserId");
        }
    }
}
