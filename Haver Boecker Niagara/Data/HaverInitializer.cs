using Haver_Boecker_Niagara.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
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
                                    PostalCode = "12345"
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
                                    PostalCode = "67890"
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
                                    PostalCode = "24680"
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
                            context.EngineeringPackages.AddRange(
                                new EngineeringPackage
                                {
                                    Engineers = new List<Engineer> { context.Engineers.Where(p => p.EngineerID == 1).FirstOrDefault() },
                                    PackageReleaseDate = DateTime.UtcNow,
                                    ApprovalDrawingDate = DateTime.UtcNow.AddDays(5)
                                },
                                new EngineeringPackage
                                {
                                    Engineers = new List<Engineer> { context.Engineers.Where(p => p.EngineerID == 2).FirstOrDefault() },
                                    PackageReleaseDate = DateTime.UtcNow,
                                    ApprovalDrawingDate = DateTime.UtcNow.AddDays(5)
                                },
                                new EngineeringPackage
                                {
                                    Engineers = new List<Engineer> {
                                        context.Engineers.Where(p => p.EngineerID == 1).FirstOrDefault(),
                                        context.Engineers.Where(p => p.EngineerID == 2).FirstOrDefault()
                                    },
                                    PackageReleaseDate = DateTime.UtcNow,
                                    ApprovalDrawingDate = DateTime.UtcNow.AddDays(5)
                                }
                            );
                            context.SaveChanges();
                            #endregion

                            #region Sales Orders
                            if (!context.SalesOrders.Any())
                            {
                                context.SalesOrders.AddRange(
                                    new SalesOrder
                                    {
                                        OrderNumber = "SO-1001",
                                        Price = 5000.00M,
                                        Status = "Confirmed",
                                        CustomerID = context.Customers.Where(p => p.CustomerID == 1).FirstOrDefault().CustomerID,
                                        AirSeal = false,
                                        Base = true,
                                        CoatingOrLining = true,
                                        Disassembly = true,
                                        Media = true,
                                        SparePartsMedia = false,
                                        EngineeringPackageID = context.EngineeringPackages.Where(p => p.EngineeringPackageID == 1).FirstOrDefault().EngineeringPackageID
                                    },
                                    new SalesOrder
                                    {
                                        OrderNumber = "SO-1002",
                                        Price = 12000.00M,
                                        Status = "Pending",
                                        CustomerID = context.Customers.Where(p => p.CustomerID == 1).FirstOrDefault().CustomerID,
                                        AirSeal = true,
                                        Base = false,
                                        CoatingOrLining = true,
                                        Disassembly = true,
                                        Media = false,
                                        SparePartsMedia = true,
                                        EngineeringPackageID = context.EngineeringPackages.Where(p => p.EngineeringPackageID == 2).FirstOrDefault().EngineeringPackageID
                                    },
                                    new SalesOrder
                                    {
                                        OrderNumber = "SO-1003",
                                        Price = 8000.00M,
                                        Status = "Pending",
                                        CustomerID = context.Customers.Where(p => p.CustomerID == 2).FirstOrDefault().CustomerID,
                                        AirSeal = true,
                                        Base = true,
                                        CoatingOrLining = true,
                                        Disassembly = true,
                                        Media = true,
                                        SparePartsMedia = true,
                                        EngineeringPackageID = context.EngineeringPackages.Where(p => p.EngineeringPackageID == 3).FirstOrDefault().EngineeringPackageID
                                    }
                                );
                                context.SaveChanges();
                            }
                            #endregion

                            #region Operations Schedules
                            if (!context.OperationsSchedules.Any())
                            {
                                var salesOrder = context.SalesOrders.FirstOrDefault();

                                if (salesOrder != null)
                                {
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
                                else
                                {
                                    Debug.WriteLine("No SalesOrder found to associate with OperationsSchedule.");
                                }
                            }
                            #endregion

                            #region Machines
                            if (!context.Machines.Any())
                            {
                                context.Machines.AddRange(
                                    new Machine
                                    {
                                        SerialNumber = "50-3964 CDN",
                                        MachineSize = 330,
                                        MachineClass = "T",
                                        MachineSizeDesc = "4' x 10' 1D",
                                        SalesOrderID = context.SalesOrders.Where(p => p.SalesOrderID == 1).FirstOrDefault().SalesOrderID,
                                        InternalPONumber = "4500805984"
                                    },
                                    new Machine
                                    {
                                        SerialNumber = "DB-1914 CDN",
                                        MachineSize = 800,
                                        MachineClass = "T",
                                        MachineSizeDesc = "6' x 20' 2D",
                                        SalesOrderID = context.SalesOrders.Where(p => p.SalesOrderID == 1).FirstOrDefault().SalesOrderID,
                                        InternalPONumber = "4500801585"
                                    },
                                    new Machine
                                    {
                                        SerialNumber = "22277 CDN",
                                        MachineSize = 1100,
                                        MachineClass = "L",
                                        MachineSizeDesc = "6' x 16' 3D",
                                        SalesOrderID = context.SalesOrders.Where(p => p.SalesOrderID == 2).FirstOrDefault().SalesOrderID,
                                        InternalPONumber = "4500805771"
                                    },
                                    new Machine
                                    {
                                        SerialNumber = "DB 1915 CDN",
                                        MachineSize = 300,
                                        MachineClass = "S",
                                        MachineSizeDesc = "5' x 9' 1D",
                                        SalesOrderID = context.SalesOrders.Where(p => p.SalesOrderID == 2).FirstOrDefault().SalesOrderID,
                                        InternalPONumber = "4500786536"
                                    },
                                    new Machine
                                    {
                                        SerialNumber = "55-1308 CDN",
                                        MachineSize = 880,
                                        MachineClass = "XL",
                                        MachineSizeDesc = "5' x 12' 2D+CP",
                                        SalesOrderID = context.SalesOrders.Where(p => p.SalesOrderID == 3).FirstOrDefault().SalesOrderID,
                                        InternalPONumber = "4500798658/799034"
                                    }
                                );
                                context.SaveChanges();
                            }
                            #endregion



                            #region Purchase Orders
                            if (!context.PurchaseOrders.Any())
                            {
                                var salesOrders = context.SalesOrders.FirstOrDefault();
                                var vendor = context.Vendors.FirstOrDefault();

                                if (salesOrders != null && vendor != null)
                                {
                                    context.PurchaseOrders.AddRange(
                                        new PurchaseOrder
                                        {
                                            PurchaseOrderNumber = "PO-1001",
                                            PODueDate = DateTime.UtcNow.AddDays(30),
                                            VendorID = vendor.VendorID,
                                            SalesOrderID = context.SalesOrders.Where(p => p.SalesOrderID == 1).FirstOrDefault().SalesOrderID,
                                        }
                                    );
                                    context.SaveChanges();
                                }
                                else
                                {
                                    Debug.WriteLine("Could not find a valid OperationsSchedule or Vendor to link the PurchaseOrder.");
                                }
                            }
                            #endregion
                        }
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