using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithDummyData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductEntries",
                columns: new[] { "Id", "CreatedAt", "EntryDateTime", "PartNumber", "Price", "ProductModel", "Quantity", "UserName" },
                values: new object[] { 1, new DateTime(2025, 11, 28, 3, 27, 32, 203, DateTimeKind.Local).AddTicks(6860), new DateTime(2025, 11, 28, 3, 27, 32, 200, DateTimeKind.Local).AddTicks(9119), "WP-3000-BLK", 299.99m, "Widget Pro 3000", 50, "John Doe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductEntries",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
