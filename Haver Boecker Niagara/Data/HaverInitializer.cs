using Haver_Boecker_Niagara.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace Haver_Boecker_Niagara.Data
{
    public static class HaverInitializer
    {
        /// <summary>
        /// Prepares the Database and seeds data as required
        /// </summary>
        /// <param name="serviceProvider">DI Container</param>
        /// <param name="DeleteDatabase">Delete the database and start from scratch</param>
        /// <param name="UseMigrations">Use Migrations or EnsureCreated</param>
        /// <param name="SeedSampleData">Add optional sample data</param>
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
                try
                {
                    if (!context.Vendors.Any())
                    {
                        context.Vendors.AddRange(
                            new Vendor { Name = "Vendor 1", ContactPerson = "John Doe", PhoneNumber = "123-456-7890", Email = "vendor1@example.com", Address = "123 Vendor St", City = "Vendor City", State = "State A", Country = "Country A", PostalCode = "12345",  CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                            new Vendor {Name = "Vendor 2", ContactPerson = "Jane Doe", PhoneNumber = "123-456-7891", Email = "vendor2@example.com", Address = "456 Vendor Ave", City = "Vendor City", State = "State B", Country = "Country B", PostalCode = "12346", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                        );
                        context.SaveChanges();
                    }

                    if (!context.Customers.Any())
                    {
                        context.Customers.AddRange(
                            new Customer { Name = "Customer 1", ContactPerson = "Alice Cooper", PhoneNumber = "234-567-8901", Email = "customer1@example.com", Address = "1 Customer Rd", City = "Customer City", State = "State A", Country = "Country A", PostalCode = "23456",  CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
                            new Customer { Name = "Customer 2", ContactPerson = "Bob Dylan", PhoneNumber = "234-567-8902", Email = "customer2@example.com", Address = "2 Customer Ave", City = "Customer City", State = "State B", Country = "Country B", PostalCode = "23457", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
                        );
                        context.SaveChanges();
                    }

                    if (!context.OperationsSchedules.Any())
                    {
                        var vendorsList = context.Vendors.ToList();
                        var customersList = context.Customers.ToList();

                        foreach (var customer in customersList)
                        {
                            for (int i = 1; i <= 3; i++)
                            {
                                var vendor = vendorsList[new Random().Next(0, vendorsList.Count)];
                                context.OperationsSchedules.Add(new OperationsSchedule
                                {
                                    SalesOrder = $"SO-{customer.CustomerID}-{i}",
                                    CustomerID = customer.CustomerID,
                                    VendorID = vendor.VendorID,
                                    MachineDescription = $"Machine Description {i}",
                                    SerialNumber = $"SN-{customer.CustomerID}-{i}",
                                    PackageReleaseDate = DateTime.UtcNow.AddDays(new Random().Next(0, 10)),
                                    PurchaseOrderNumber = $"PO-{i}",
                                    PODueDate = DateTime.UtcNow.AddDays(new Random().Next(1, 10)),
                                    DeliveryDate = DateTime.UtcNow.AddDays(new Random().Next(10, 20)),
                                    Media = "Standard",
                                    SparePartsMedia = "Standard",
                                    Base = "Base Description",
                                    AirSeal = "AirSeal Description",
                                    CoatingOrLining = "Coating Description",
                                    Disassembly = "Disassembly Description",
                                    Notes = $"Notes for SO-{i}",
                                    Customer = customer,
                                    Vendor = vendor
                                });
                            }
                        }
                        context.SaveChanges();
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
                    Random random = new Random();

                    try
                    {
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

