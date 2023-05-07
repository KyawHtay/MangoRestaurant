using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartAPI.Migrations
{
    public partial class UpdateCartHeaderIdinCartDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeader_CardHeaderId",
                table: "CartDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartHeader",
                table: "CartHeader");

            migrationBuilder.RenameTable(
                name: "CartHeader",
                newName: "CartHeaders");

            migrationBuilder.RenameColumn(
                name: "CardHeaderId",
                table: "CartDetails",
                newName: "CartHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CardHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_CartHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartHeaders",
                table: "CartHeaders",
                column: "CartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails",
                column: "CartHeaderId",
                principalTable: "CartHeaders",
                principalColumn: "CartHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_CartHeaders_CartHeaderId",
                table: "CartDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartHeaders",
                table: "CartHeaders");

            migrationBuilder.RenameTable(
                name: "CartHeaders",
                newName: "CartHeader");

            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartDetails",
                newName: "CardHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_CartDetails_CartHeaderId",
                table: "CartDetails",
                newName: "IX_CartDetails_CardHeaderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartHeader",
                table: "CartHeader",
                column: "CartHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_CartHeader_CardHeaderId",
                table: "CartDetails",
                column: "CardHeaderId",
                principalTable: "CartHeader",
                principalColumn: "CartHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
