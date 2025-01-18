using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haver_Boecker_Niagara.Migrations.Haver
{
    /// <inheritdoc />
    public partial class FixRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "OperationsSchedules");

            migrationBuilder.DropColumn(
                name: "VendorName",
                table: "OperationsSchedules");

            migrationBuilder.AddColumn<int>(
                name: "VendorID1",
                table: "OperationsSchedules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Customers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OperationsSchedules_VendorID1",
                table: "OperationsSchedules",
                column: "VendorID1");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationsSchedules_Vendors_VendorID1",
                table: "OperationsSchedules",
                column: "VendorID1",
                principalTable: "Vendors",
                principalColumn: "VendorID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationsSchedules_Vendors_VendorID1",
                table: "OperationsSchedules");

            migrationBuilder.DropIndex(
                name: "IX_OperationsSchedules_VendorID1",
                table: "OperationsSchedules");

            migrationBuilder.DropColumn(
                name: "VendorID1",
                table: "OperationsSchedules");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "OperationsSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VendorName",
                table: "OperationsSchedules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
