using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Haver_Boecker_Niagara.Data.FMMigrations
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
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Engineers",
                columns: table => new
                {
                    EngineerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Initials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engineers", x => x.EngineerID);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                columns: table => new
                {
                    VendorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.VendorID);
                });

            migrationBuilder.CreateTable(
                name: "GanttSchedules",
                columns: table => new
                {
                    GanttID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    EngineerID = table.Column<int>(type: "int", nullable: false),
                    MachineDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromiseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeadlineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID1 = table.Column<int>(type: "int", nullable: true),
                    EngineerID1 = table.Column<int>(type: "int", nullable: true)
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
                    OperarionsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    EngineerID = table.Column<int>(type: "int", nullable: false),
                    MachineDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PODueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalDrawingRelease = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PackageReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ITPRequirements = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreOrderInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BudgetedAssemblyHours = table.Column<int>(type: "int", nullable: true),
                    ActualAssemblyHours = table.Column<int>(type: "int", nullable: true),
                    ReworkHours = table.Column<int>(type: "int", nullable: true),
                    ProductionOrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameplateStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackagingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerID1 = table.Column<int>(type: "int", nullable: true),
                    EngineerID1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationsSchedules", x => x.OperarionsID);
                    table.ForeignKey(
                        name: "FK_OperationsSchedules_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationsSchedules_Customers_CustomerID1",
                        column: x => x.CustomerID1,
                        principalTable: "Customers",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_OperationsSchedules_Engineers_EngineerID",
                        column: x => x.EngineerID,
                        principalTable: "Engineers",
                        principalColumn: "EngineerID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OperationsSchedules_Engineers_EngineerID1",
                        column: x => x.EngineerID1,
                        principalTable: "Engineers",
                        principalColumn: "EngineerID");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalDrawings",
                columns: table => new
                {
                    DrawingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    BOM_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    BOM_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BOM_Details = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    MeetingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Participants = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KeyDiscussions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    MilestoneID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MilestoneName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProjectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SpecialNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    NCR_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    NCR_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolutionStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProgressNotes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LateFlag = table.Column<bool>(type: "bit", nullable: false),
                    DoneFlag = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateChanged = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "AssemblyLogs",
                columns: table => new
                {
                    AssemblyLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    AssemblyStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssemblyEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActualAssemblyHours = table.Column<int>(type: "int", nullable: true),
                    ReworkHours = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssemblyLogs", x => x.AssemblyLogID);
                    table.ForeignKey(
                        name: "FK_AssemblyLogs_OperationsSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "OperationsSchedules",
                        principalColumn: "OperarionsID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcurementLogs",
                columns: table => new
                {
                    ProcurementLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    VendorID = table.Column<int>(type: "int", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcurementLogs", x => x.ProcurementLogID);
                    table.ForeignKey(
                        name: "FK_ProcurementLogs_OperationsSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "OperationsSchedules",
                        principalColumn: "OperarionsID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcurementLogs_Vendors_VendorID",
                        column: x => x.VendorID,
                        principalTable: "Vendors",
                        principalColumn: "VendorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityLogs",
                columns: table => new
                {
                    QualityLogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ProductionOrder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QualityChecks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssuesFound = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReworkRequired = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityLogs", x => x.QualityLogID);
                    table.ForeignKey(
                        name: "FK_QualityLogs_OperationsSchedules_OrderID",
                        column: x => x.OrderID,
                        principalTable: "OperationsSchedules",
                        principalColumn: "OperarionsID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalDrawings_OrderID",
                table: "ApprovalDrawings",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_AssemblyLogs_OrderID",
                table: "AssemblyLogs",
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
                name: "IX_OperationsSchedules_CustomerID1",
                table: "OperationsSchedules",
                column: "CustomerID1");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsSchedules_EngineerID",
                table: "OperationsSchedules",
                column: "EngineerID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsSchedules_EngineerID1",
                table: "OperationsSchedules",
                column: "EngineerID1");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementLogs_OrderID",
                table: "ProcurementLogs",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProcurementLogs_VendorID",
                table: "ProcurementLogs",
                column: "VendorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressLogs_OrderID",
                table: "ProgressLogs",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_QualityLogs_OrderID",
                table: "QualityLogs",
                column: "OrderID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovalDrawings");

            migrationBuilder.DropTable(
                name: "AssemblyLogs");

            migrationBuilder.DropTable(
                name: "BOMs");

            migrationBuilder.DropTable(
                name: "KickoffMeetings");

            migrationBuilder.DropTable(
                name: "Milestones");

            migrationBuilder.DropTable(
                name: "NCRs");

            migrationBuilder.DropTable(
                name: "ProcurementLogs");

            migrationBuilder.DropTable(
                name: "ProgressLogs");

            migrationBuilder.DropTable(
                name: "QualityLogs");

            migrationBuilder.DropTable(
                name: "Vendors");

            migrationBuilder.DropTable(
                name: "GanttSchedules");

            migrationBuilder.DropTable(
                name: "OperationsSchedules");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Engineers");
        }
    }
}
