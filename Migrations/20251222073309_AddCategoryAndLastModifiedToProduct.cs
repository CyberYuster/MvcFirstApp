using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvcDatabaseApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndLastModifiedToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "CreateDate", "LastModified" },
                values: new object[] { "Accessories", new DateTime(2025, 12, 22, 7, 33, 4, 599, DateTimeKind.Utc).AddTicks(2611), null });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "CreateDate", "LastModified" },
                values: new object[] { "Electronics", new DateTime(2025, 12, 22, 7, 33, 4, 599, DateTimeKind.Utc).AddTicks(2617), null });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Products");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 12, 3, 18, 47, 58, 248, DateTimeKind.Utc).AddTicks(9299));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 12, 3, 18, 47, 58, 248, DateTimeKind.Utc).AddTicks(9302));
        }
    }
}
