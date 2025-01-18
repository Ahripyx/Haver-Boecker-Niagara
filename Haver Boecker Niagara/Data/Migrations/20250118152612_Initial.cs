using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haver_Boecker_Niagara.Data.Migrations
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
                    ContactPerson = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Engineers",
                columns: table => new
                {
                    EngineerID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Initials = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engineers", x => x.EngineerID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ContactPerson = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: false),
                    State = table.Column<string>(type: "TEXT", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "GanttSchedules",
                columns: table => new
                {
                    GanttID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: false),
                    EngineerID = table.Column<int>(type: "INTEGER", nullable: false),
                    MachineDetails = table.Column<string>(type: "TEXT", nullable: false),
                    PromiseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DeadlineDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerID1 = table.Column<int>(type: "INTEGER", nullable: true),
                    EngineerID1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GanttSchedules", x => x.GanttID);
                    table.ForeignKey(
                        name: "FK_GanttSchedules_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GanttSchedules_Customers_CustomerID1",
                        column: x => x.CustomerID1,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_GanttSchedules_Engineers_EngineerID",
                        column: x => x.EngineerID,
                        principalTable: "Engineers",
                        principalColumn: "EngineerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GanttSchedules_Engineers_EngineerID1",
                        column: x => x.EngineerID1,
                        principalTable: "Engineers",
                        principalColumn: "EngineerID");
                });

            migrationBuilder.CreateTable(
                name: "OperationsSchedules",
                columns: table => new
                {
                    OperationsID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SalesOrder = table.Column<string>(type: "TEXT", nullable: false),
                    CustomerID = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", nullable: false),
                    MachineDescription = table.Column<string>(type: "TEXT", nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PackageReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    VendorID = table.Column<int>(type: "INTEGER", nullable: false),
                    VendorName = table.Column<string>(type: "TEXT", nullable: false),
                    PurchaseOrderNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PODueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Media = table.Column<string>(type: "TEXT", nullable: false),
                    SparePartsMedia = table.Column<string>(type: "TEXT", nullable: false),
                    Base = table.Column<string>(type: "TEXT", nullable: false),
                    AirSeal = table.Column<string>(type: "TEXT", nullable: false),
                    CoatingOrLining = table.Column<string>(type: "TEXT", nullable: false),
                    Disassembly = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationsSchedules", x => x.OperationsID);
                    table.ForeignKey(
                        name: "FK_OperationsSchedules_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsSchedules_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalDrawings",
                columns: table => new
                {
                    DrawingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalDrawings", x => x.DrawingID);
                    table.ForeignKey(
                        name: "FK_ApprovalDrawings_GanttSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "GanttSchedules",
                        principalColumn: "GanttID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BOMs",
                columns: table => new
                {
                    BOM_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    BOM_Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BOM_Details = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOMs", x => x.BOM_ID);
                    table.ForeignKey(
                        name: "FK_BOMs_GanttSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "GanttSchedules",
                        principalColumn: "GanttID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KickoffMeetings",
                columns: table => new
                {
                    MeetingID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Participants = table.Column<string>(type: "TEXT", nullable: false),
                    KeyDiscussions = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KickoffMeetings", x => x.MeetingID);
                    table.ForeignKey(
                        name: "FK_KickoffMeetings_GanttSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "GanttSchedules",
                        principalColumn: "GanttID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Milestones",
                columns: table => new
                {
                    MilestoneID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    MilestoneName = table.Column<string>(type: "TEXT", nullable: false),
                    PlannedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProjectedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    SpecialNotes = table.Column<string>(type: "TEXT", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milestones", x => x.MilestoneID);
                    table.ForeignKey(
                        name: "FK_Milestones_GanttSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "GanttSchedules",
                        principalColumn: "GanttID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NCRs",
                columns: table => new
                {
                    NCR_ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: true),
                    NCR_Number = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResolutionStatus = table.Column<string>(type: "TEXT", nullable: false),
                    IssuedBy = table.Column<string>(type: "TEXT", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NCRs", x => x.NCR_ID);
                    table.ForeignKey(
                        name: "FK_NCRs_GanttSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "GanttSchedules",
                        principalColumn: "GanttID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProgressNotes = table.Column<string>(type: "TEXT", nullable: false),
                    LateFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    DoneFlag = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressLogs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_ProgressLogs_GanttSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "GanttSchedules",
                        principalColumn: "GanttID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDrawings_OrderID",
                table: "ApprovalDrawings",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BOMs_OrderID",
                table: "BOMs",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_GanttSchedules_CustomerID",
                table: "GanttSchedules",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_GanttSchedules_CustomerID1",
                table: "GanttSchedules",
                column: "CustomerID1");

            migrationBuilder.CreateIndex(
                name: "IX_GanttSchedules_EngineerID",
                table: "GanttSchedules",
                column: "EngineerID");

            migrationBuilder.CreateIndex(
                name: "IX_GanttSchedules_EngineerID1",
                table: "GanttSchedules",
                column: "EngineerID1");

            migrationBuilder.CreateIndex(
                name: "IX_KickoffMeetings_OrderID",
                table: "KickoffMeetings",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Milestones_OrderID",
                table: "Milestones",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_NCRs_OrderID",
                table: "NCRs",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsSchedules_CustomerID",
                table: "OperationsSchedules",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsSchedules_VendorID",
                table: "OperationsSchedules",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressLogs_OrderID",
                table: "ProgressLogs",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalDrawings");

            migrationBuilder.DropTable(
                name: "BOMs");

            migrationBuilder.DropTable(
                name: "KickoffMeetings");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "NCRs");

            migrationBuilder.DropTable(
                name: "OperationsSchedules");

            migrationBuilder.DropTable(
                name: "ProgressLogs");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "GanttSchedules");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Engineers");
        }
    }
}
