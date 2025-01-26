using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haver_Boecker_Niagara.Data.MachinesMigration
{
    /// <inheritdoc />
    public partial class MachinesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_EngineeringPackages_EngineeringPackageID1",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_EngineeringPackageID1",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "EngineeringPackageID1",
                table: "SalesOrders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EngineeringPackageID1",
                table: "SalesOrders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_EngineeringPackageID1",
                table: "SalesOrders",
                column: "EngineeringPackageID1");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_EngineeringPackages_EngineeringPackageID1",
                table: "SalesOrders",
                column: "EngineeringPackageID1",
                principalTable: "EngineeringPackages",
                principalColumn: "EngineeringPackageID");
        }
    }
}
