using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.Email.Migrations
{
    /// <inheritdoc />
    public partial class updateEmailLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Emailogs",
                table: "Emailogs");

            migrationBuilder.RenameTable(
                name: "Emailogs",
                newName: "Emaillogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emaillogs",
                table: "Emaillogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Emaillogs",
                table: "Emaillogs");

            migrationBuilder.RenameTable(
                name: "Emaillogs",
                newName: "Emailogs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Emailogs",
                table: "Emailogs",
                column: "Id");
        }
    }
}
