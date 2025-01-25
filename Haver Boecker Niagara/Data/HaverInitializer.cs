﻿using Haver_Boecker_Niagara.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Haver_Boecker_Niagara.Data
{
    public static class HaverInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new HaverContext(
                serviceProvider.GetRequiredService<DbContextOptions<HaverContext>>()))
            {
                #region Prepare the Database
                try
                {
                    if (UseMigrations)
                    {
                        if (DeleteDatabase)
                        {
                            context.Database.EnsureDeleted();
                        }
                        context.Database.Migrate();
                    }
                    else
                    {
                        if (DeleteDatabase)
                        {
                            context.Database.EnsureDeleted();
                            context.Database.EnsureCreated();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                #region Seed Required Data
                // Empty for now
                #endregion

                #region Seed Sample Data
                if (SeedSampleData)
                {
                    try
                    {
                        #region Customers
                        if (!context.Customers.Any())
                        {
                            context.Customers.AddRange(
                                new Customer
                                {
                                    Name = "Acme Corporation",
                                    ContactFirstName = "John",
                                    ContactLastName = "Doe",
                                    PhoneNumber = "123-456-7890",
                                    Email = "contact@acmecorp.com",
                                    Address = "123 Acme St.",
                                    City = "Metropolis",
                                    Country = "USA",
                                    PostalCode = "12345",
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                },
                                new Customer
                                {
                                    Name = "Beta Industries",
                                    ContactFirstName = "Jane",
                                    ContactLastName = "Smith",
                                    PhoneNumber = "987-654-3210",
                                    Email = "support@betaind.com",
                                    Address = "456 Beta Ave.",
                                    City = "Gotham",
                                    Country = "USA",
                                    PostalCode = "67890",
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        #region Vendors
                        if (!context.Vendors.Any())
                        {
                            context.Vendors.AddRange(
                                new Vendor
                                {
                                    Name = "Global Parts Inc.",
                                    ContactFirstName = "Tom",
                                    ContactLastName = "Green",
                                    PhoneNumber = "111-222-3333",
                                    Email = "sales@globalparts.com",
                                    Address = "789 Global Rd.",
                                    City = "Sunnyvale",
                                    Country = "USA",
                                    PostalCode = "24680",
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        #region Engineers
                        if (!context.Engineers.Any())
                        {
                            context.Engineers.AddRange(
                                new Engineer
                                {
                                    FirstName = "Alice",
                                    LastName = "Johnson",
                                    Email = "alice.johnson@hbn.com"
                                },
                                new Engineer
                                {
                                    FirstName = "Bob",
                                    LastName = "Martin",
                                    Email = "bob.martin@hbn.com"
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        #region Engineering Packages
                        if (!context.EngineeringPackages.Any())
                        {
                            var engineer = context.Engineers.FirstOrDefault();
                            if (engineer != null)
                            {
                                context.EngineeringPackages.AddRange(
                                    new EngineeringPackage
                                    {
                                        Engineers = new List<Engineer> { engineer },
                                        PackageReleaseDate = DateTime.UtcNow,
                                        ApprovalDrawingDate = DateTime.UtcNow.AddDays(5),

                                    }
                                );
                                context.SaveChanges();
                            }
                        }
                        #endregion

                        #region Sales Orders
                        if (!context.SalesOrders.Any())
                        {
                            var customer = context.Customers.FirstOrDefault();
                            var engPackage = context.EngineeringPackages.FirstOrDefault();
                            context.SalesOrders.AddRange(
                                new SalesOrder
                                {
                                    OrderNumber = "SO-1001",
                                    Price = 5000.00M,
                                    Status = "Confirmed",
                                    CustomerID = customer.CustomerID,
                                    EngineeringPackageID = engPackage.EngineeringPackageID
                                },
                                new SalesOrder
                                {
                                    OrderNumber = "SO-1002",
                                    Price = 12000.00M,
                                    Status = "Pending",
                                    CustomerID = customer.CustomerID,
                                    EngineeringPackageID = engPackage.EngineeringPackageID
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        #region Operations Schedules
                        if (!context.OperationsSchedules.Any())
                        {
                            var salesOrder = context.SalesOrders.FirstOrDefault();
                            context.OperationsSchedules.AddRange(
                                new OperationsSchedule
                                {
                                    SalesOrderID = salesOrder.SalesOrderID,
                                    DeliveryDate = DateTime.UtcNow.AddMonths(1),
                                    ExtraNotes = "Initial assembly scheduled.",
                                    NamePlateStatus = false
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        #region Machines
                        if (!context.Machines.Any())
                        {
                            var salesOrder = context.SalesOrders.FirstOrDefault();
                            context.Machines.AddRange(
                                new Machine
                                {
                                    SerialNumber = "M12345",
                                    MachineSize = 2,
                                    MachineClass = "Heavy Duty",
                                    MachineSizeDesc = "Large",
                                    SalesOrderID = salesOrder.SalesOrderID,
                                    AirSeal = false,
                                    Base = true,
                                    CoatingOrLining = true,
                                    Disassembly = false,
                                    InternalPONumber = "1234567",
                                    Media = false,
                                    SparePartsMedia = true
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        #region Purchase Orders
                        if (!context.PurchaseOrders.Any())
                        {
                            var operationsSchedule = context.OperationsSchedules.FirstOrDefault();
                            var vendor = context.Vendors.FirstOrDefault();
                            context.PurchaseOrders.AddRange(
                                new PurchaseOrder
                                {
                                    OperationsID = operationsSchedule.OperationsID,
                                    PurchaseOrderNumber = "PO-1001",
                                    PODueDate = DateTime.UtcNow.AddDays(30),
                                    VendorID = vendor.VendorID
                                }
                            );
                            context.SaveChanges();
                        }
                        #endregion

                        
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }
                }
                #endregion
            }
        }
    }
}
