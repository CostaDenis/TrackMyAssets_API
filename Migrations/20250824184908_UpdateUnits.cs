using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackMyAssets_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUnits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: new Guid("4615c7c6-cd30-4afc-ac3a-759ededfcf7f"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Units",
                table: "UserAssets",
                type: "DECIMAL(18,8)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitsChanged",
                table: "AssetTransactions",
                type: "DECIMAL(18,8)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,2)");

            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "Id", "Email", "Password" },
                values: new object[] { new Guid("b1de5536-f740-480b-a686-76ae1c39f53c"), "adm@teste.com", "AQAAAAIAAYagAAAAEC9OuKj8Axx4BT2qIe47xaon8XM1Nyv2HW38v30wSNL+JAmH4c3pl9ufIIX0bSoVJA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrators",
                keyColumn: "Id",
                keyValue: new Guid("b1de5536-f740-480b-a686-76ae1c39f53c"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Units",
                table: "UserAssets",
                type: "DECIMAL(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,8)",
                oldDefaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitsChanged",
                table: "AssetTransactions",
                type: "DECIMAL(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(18,8)");

            migrationBuilder.InsertData(
                table: "Administrators",
                columns: new[] { "Id", "Email", "Password" },
                values: new object[] { new Guid("4615c7c6-cd30-4afc-ac3a-759ededfcf7f"), "adm@teste.com", "AQAAAAIAAYagAAAAEC9OuKj8Axx4BT2qIe47xaon8XM1Nyv2HW38v30wSNL+JAmH4c3pl9ufIIX0bSoVJA==" });
        }
    }
}
