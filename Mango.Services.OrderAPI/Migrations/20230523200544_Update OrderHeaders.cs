using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderHeaders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeader_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHeader",
                table: "OrderHeader");

            migrationBuilder.RenameTable(
                name: "OrderHeader",
                newName: "OrderHeaders");

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderTime",
                table: "OrderHeaders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "PaymentStatus",
                table: "OrderHeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHeaders",
                table: "OrderHeaders",
                column: "OrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeaders",
                principalColumn: "OrderHeaderId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderHeaders_OrderHeaderId",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderHeaders",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "OrderTime",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderHeaders");

            migrationBuilder.RenameTable(
                name: "OrderHeaders",
                newName: "OrderHeader");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderHeader",
                table: "OrderHeader",
                column: "OrderHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderHeader_OrderHeaderId",
                table: "OrderDetails",
                column: "OrderHeaderId",
                principalTable: "OrderHeader",
                principalColumn: "OrderHeaderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
