using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contactly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConnectionTrackerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConnectionTracker",
                columns: table => new
                {
                    ConnectionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ContactId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionTracker", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_ConnectionTracker_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConnectionTracker_ContactId",
                table: "ConnectionTracker",
                column: "ContactId",
                unique: true,
                filter: "[ContactId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConnectionTracker");
        }
    }
}
