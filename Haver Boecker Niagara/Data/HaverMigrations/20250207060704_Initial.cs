using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haver_Boecker_Niagara.Data.HaverMigrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ContactFirstName = table.Column<string>(type: "TEXT", nullable: true),
                    ContactLastName = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "EngineeringPackages",
                columns: table => new
                {
                    EngineeringPackageID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PackageReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ApprovalDrawingDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualPackageReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualApprovalDrawingDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineeringPackages", x => x.EngineeringPackageID);
                });

            migrationBuilder.CreateTable(
                name: "Engineers",
                columns: table => new
                {
                    EngineerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engineers", x => x.EngineerID);
                });

            migrationBuilder.CreateTable(
                name: "Machines",
                columns: table => new
                {
                    MachineID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    InternalPONumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NamePlateStatus = table.Column<bool>(type: "INTEGER", nullable: false),
                    MachineSize = table.Column<int>(type: "INTEGER", nullable: false),
                    MachineClass = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MachineSizeDesc = table.Column<string>(type: "TEXT", nullable: false),
                    Media = table.Column<bool>(type: "INTEGER", nullable: false),
                    SparePartsMedia = table.Column<bool>(type: "INTEGER", nullable: false),
                    Base = table.Column<bool>(type: "INTEGER", nullable: false),
                    AirSeal = table.Column<bool>(type: "INTEGER", nullable: false),
                    CoatingOrLining = table.Column<bool>(type: "INTEGER", nullable: false),
                    Disassembly = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreOrderNotes = table.Column<string>(type: "TEXT", nullable: true),
                    ScopeNotes = table.Column<string>(type: "TEXT", nullable: true),
                    ActualAssemblyHours = table.Column<string>(type: "TEXT", nullable: true),
                    ActualReworkHours = table.Column<string>(type: "TEXT", nullable: true),
                    BudgetedAssemblyHours = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Machines", x => x.MachineID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ContactFirstName = table.Column<string>(type: "TEXT", nullable: true),
                    ContactLastName = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    SalesOrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualCompletionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ExtraNotes = table.Column<string>(type: "TEXT", nullable: true),
                    EngineeringPackageID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesOrders", x => x.SalesOrderID);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrders_EngineeringPackages_EngineeringPackageID",
                        column: x => x.EngineeringPackageID,
                        principalTable: "EngineeringPackages",
                        principalColumn: "EngineeringPackageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EngineeringPackageEngineers",
                columns: table => new
                {
                    EngineeringPackageEngineerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EngineerID = table.Column<int>(type: "INTEGER", nullable: false),
                    EngineeringPackageID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineeringPackageEngineers", x => x.EngineeringPackageEngineerID);
                    table.ForeignKey(
                        name: "FK_EngineeringPackageEngineers_EngineeringPackages_EngineeringPackageID",
                        column: x => x.EngineeringPackageID,
                        principalTable: "EngineeringPackages",
                        principalColumn: "EngineeringPackageID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EngineeringPackageEngineers_Engineers_EngineerID",
                        column: x => x.EngineerID,
                        principalTable: "Engineers",
                        principalColumn: "EngineerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineSalesOrder",
                columns: table => new
                {
                    MachineSalesOrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MachineID = table.Column<int>(type: "INTEGER", nullable: false),
                    SalesOrderID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineSalesOrder", x => x.MachineSalesOrderID);
                    table.ForeignKey(
                        name: "FK_MachineSalesOrder_Machines_MachineID",
                        column: x => x.MachineID,
                        principalTable: "Machines",
                        principalColumn: "MachineID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineSalesOrder_SalesOrders_SalesOrderID",
                        column: x => x.SalesOrderID,
                        principalTable: "SalesOrders",
                        principalColumn: "SalesOrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    PurchaseOrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PurchaseOrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PODueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    POActualDueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VendorID = table.Column<int>(type: "INTEGER", nullable: true),
                    SalesOrderID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.PurchaseOrderID);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_SalesOrders_SalesOrderID",
                        column: x => x.SalesOrderID,
                        principalTable: "SalesOrders",
                        principalColumn: "SalesOrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringPackageEngineers_EngineerID",
                table: "EngineeringPackageEngineers",
                column: "EngineerID");

            migrationBuilder.CreateIndex(
                name: "IX_EngineeringPackageEngineers_EngineeringPackageID",
                table: "EngineeringPackageEngineers",
                column: "EngineeringPackageID");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSalesOrder_MachineID",
                table: "MachineSalesOrder",
                column: "MachineID");

            migrationBuilder.CreateIndex(
                name: "IX_MachineSalesOrder_SalesOrderID",
                table: "MachineSalesOrder",
                column: "SalesOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SalesOrderID",
                table: "PurchaseOrders",
                column: "SalesOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_VendorID",
                table: "PurchaseOrders",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CustomerID",
                table: "SalesOrders",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_EngineeringPackageID",
                table: "SalesOrders",
                column: "EngineeringPackageID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EngineeringPackageEngineers");

            migrationBuilder.DropTable(
                name: "MachineSalesOrder");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "Engineers");

            migrationBuilder.DropTable(
                name: "Machines");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "EngineeringPackages");
        }
    }
}
