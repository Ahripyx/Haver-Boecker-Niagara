﻿// <auto-generated />
using System;
using Haver_Boecker_Niagara.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Haver_Boecker_Niagara.Data.IdentityMigrations
{
    [DbContext(typeof(HaverContext))]
    partial class HaverContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactFirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactLastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Engineer", b =>
                {
                    b.Property<int>("EngineerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EngineerID");

                    b.ToTable("Engineers");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.EngineeringPackage", b =>
                {
                    b.Property<int>("EngineeringPackageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ActualApprovalDrawingDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ActualPackageReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ApprovalDrawingDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PackageReleaseDate")
                        .HasColumnType("TEXT");

                    b.HasKey("EngineeringPackageID");

                    b.ToTable("EngineeringPackages");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.EngineeringPackageEngineer", b =>
                {
                    b.Property<int>("EngineeringPackageEngineerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EngineerID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EngineeringPackageID")
                        .HasColumnType("INTEGER");

                    b.HasKey("EngineeringPackageEngineerID");

                    b.HasIndex("EngineerID");

                    b.HasIndex("EngineeringPackageID");

                    b.ToTable("EngineeringPackageEngineers");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.GanttSchedule", b =>
                {
                    b.Property<int>("GanttID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DeadlineDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EngineeringOnly")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LatestMilestone")
                        .HasColumnType("TEXT");

                    b.Property<int?>("MachineID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NCR")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PreOrdersExpected")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PromiseDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReadinessToShipExpected")
                        .HasColumnType("TEXT");

                    b.Property<int>("SalesOrderID")
                        .HasColumnType("INTEGER");

                    b.HasKey("GanttID");

                    b.HasIndex("SalesOrderID");

                    b.ToTable("GanttSchedules");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.KickoffMeeting", b =>
                {
                    b.Property<int>("MeetingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GanttID")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("MeetingDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("MeetingSummary")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("TEXT");

                    b.HasKey("MeetingID");

                    b.HasIndex("GanttID");

                    b.ToTable("KickoffMeetings");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Machine", b =>
                {
                    b.Property<int>("MachineID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ActualAssemblyHours")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ActualReworkHours")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AirSeal")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Base")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BudgetedAssemblyHours")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CoatingOrLining")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Disassembly")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InternalPONumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("MachineClass")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("MachineSize")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MachineSizeDesc")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Media")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("NamePlateStatus")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PreOrderNotes")
                        .HasColumnType("TEXT");

                    b.Property<string>("ScopeNotes")
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("SparePartsMedia")
                        .HasColumnType("INTEGER");

                    b.HasKey("MachineID");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.MachineSalesOrder", b =>
                {
                    b.Property<int>("MachineSalesOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MachineID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SalesOrderID")
                        .HasColumnType("INTEGER");

                    b.HasKey("MachineSalesOrderID");

                    b.HasIndex("MachineID");

                    b.HasIndex("SalesOrderID");

                    b.ToTable("MachineSalesOrders");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Milestone", b =>
                {
                    b.Property<int>("MilestoneID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ActualCompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("KickOfMeetingID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Name")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("MilestoneID");

                    b.HasIndex("KickOfMeetingID");

                    b.ToTable("Milestones");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("PurchaseOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("POActualDueDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PODueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PurchaseOrderNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SalesOrderID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("VendorID")
                        .HasColumnType("INTEGER");

                    b.HasKey("PurchaseOrderID");

                    b.HasIndex("SalesOrderID");

                    b.HasIndex("VendorID");

                    b.ToTable("PurchaseOrders");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.SalesOrder", b =>
                {
                    b.Property<int>("SalesOrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ActualCompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EngineeringPackageID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExtraNotes")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("SalesOrderID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("EngineeringPackageID")
                        .IsUnique();

                    b.ToTable("SalesOrders");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Vendor", b =>
                {
                    b.Property<int>("VendorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactFirstName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ContactLastName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PostalCode")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("VendorID");

                    b.ToTable("Vendors");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.EngineeringPackageEngineer", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.Engineer", "Engineer")
                        .WithMany()
                        .HasForeignKey("EngineerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Haver_Boecker_Niagara.Models.EngineeringPackage", "EngineeringPackage")
                        .WithMany()
                        .HasForeignKey("EngineeringPackageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Engineer");

                    b.Navigation("EngineeringPackage");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.GanttSchedule", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.SalesOrder", "SalesOrder")
                        .WithMany("GanttSchedules")
                        .HasForeignKey("SalesOrderID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SalesOrder");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.KickoffMeeting", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.GanttSchedule", "GanttSchedule")
                        .WithMany("KickoffMeetings")
                        .HasForeignKey("GanttID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GanttSchedule");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.MachineSalesOrder", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Haver_Boecker_Niagara.Models.SalesOrder", "SalesOrder")
                        .WithMany()
                        .HasForeignKey("SalesOrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Machine");

                    b.Navigation("SalesOrder");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Milestone", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.KickoffMeeting", "KickoffMeeting")
                        .WithMany("Milestones")
                        .HasForeignKey("KickOfMeetingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KickoffMeeting");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.PurchaseOrder", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.SalesOrder", "SalesOrder")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("SalesOrderID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Haver_Boecker_Niagara.Models.Vendor", "Vendor")
                        .WithMany("PurchaseOrders")
                        .HasForeignKey("VendorID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("SalesOrder");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.SalesOrder", b =>
                {
                    b.HasOne("Haver_Boecker_Niagara.Models.Customer", "Customer")
                        .WithMany("SaleOrders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Haver_Boecker_Niagara.Models.EngineeringPackage", "EngineeringPackage")
                        .WithOne("SalesOrder")
                        .HasForeignKey("Haver_Boecker_Niagara.Models.SalesOrder", "EngineeringPackageID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Customer");

                    b.Navigation("EngineeringPackage");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Customer", b =>
                {
                    b.Navigation("SaleOrders");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.EngineeringPackage", b =>
                {
                    b.Navigation("SalesOrder");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.GanttSchedule", b =>
                {
                    b.Navigation("KickoffMeetings");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.KickoffMeeting", b =>
                {
                    b.Navigation("Milestones");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.SalesOrder", b =>
                {
                    b.Navigation("GanttSchedules");

                    b.Navigation("PurchaseOrders");
                });

            modelBuilder.Entity("Haver_Boecker_Niagara.Models.Vendor", b =>
                {
                    b.Navigation("PurchaseOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
