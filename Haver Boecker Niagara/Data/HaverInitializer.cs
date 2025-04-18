using Haver_Boecker_Niagara.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Task = Haver_Boecker_Niagara.Models.Task;

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

                if (SeedSampleData)
                {
                    try
                    {
                        string[] firstNames = { "John", "Michael", "Emily", "Sarah", "David", "James", "Sophia", "Daniel", "Olivia", "Robert" };
                        string[] lastNames = { "Smith", "Johnson", "Brown", "Davis", "Miller", "Wilson", "Moore", "Anderson", "Taylor", "Thomas" };
                        string[] cities = { "New York", "Los Angeles", "Chicago", "Houston", "San Francisco", "London", "Berlin", "Toronto", "Sydney", "Paris" };
                        string[] countries = { "USA", "UK", "Germany", "Canada", "Australia", "France", "Italy", "Spain", "Netherlands", "Sweden" };

                        if (!context.Customers.Any())
                        {
                            string[] companyNames = { "Acme Corp", "BrightTech Ltd", "Summit Solutions", "Global Enterprises", "Pioneer Logistics",
                              "Vertex Systems", "Nexus Industries", "PrimeTech Solutions", "Western Dynamics", "Liberty Innovations" };
                            for (int i = 0; i < 10; i++)
                            {
                                context.Customers.Add(new Customer
                                {
                                    Name = companyNames[i],
                                    ContactFirstName = firstNames[i],
                                    ContactLastName = lastNames[i],
                                    PhoneNumber = $"555-100{i}",
                                    Email = $"customer{i + 1}@example.com",
                                    Address = $"{i + 1} Elm St",
                                    City = cities[i],
                                    Country = countries[i],
                                    PostalCode = $"1000{i}"
                                });
                            }
                            context.SaveChanges();
                        }
                        if (!context.Vendors.Any())
                        {
                            string[] vendorNames = { "Tech Supplies Inc.", "Global Trade Ltd.", "Prime Distributors", "Nexus Components", "Pioneer Solutions",
                             "Western Tools", "Omega Equipments", "Vertex Systems", "Summit Industries", "Liberty Parts" };

                            for (int i = 0; i < 10; i++)
                            {
                                context.Vendors.Add(new Vendor
                                {
                                    Name = vendorNames[i],
                                    ContactFirstName = firstNames[i],
                                    ContactLastName = lastNames[i],
                                    PhoneNumber = $"666-200{i}",
                                    Email = $"vendor{i + 1}@example.com",
                                    Address = $"{i + 1} Business Rd",
                                    City = cities[i],
                                    Country = countries[i],
                                    PostalCode = $"2000{i}"
                                });
                            }
                            context.SaveChanges();
                        }

                        if (!context.Engineers.Any())
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                context.Engineers.Add(new Engineer
                                {
                                    FirstName = firstNames[i],
                                    LastName = lastNames[i],
                                    Email = $"engineer{i + 1}@hbn.com"
                                });
                            }
                            context.SaveChanges();
                        }
                        if (!context.EngineeringPackages.Any())
                        {
                            var engineers = context.Engineers.ToList();
                            for (int i = 1; i <= 10; i++)
                            {
                                context.EngineeringPackages.Add(new EngineeringPackage
                                {
                                    Engineers = engineers.Take(2).ToList(),
                                    PackageReleaseDate = DateTime.UtcNow,
                                    ApprovalDrawingDate = DateTime.UtcNow.AddDays(5)
                                });
                            }
                            context.SaveChanges();
                        }

                        if (!context.SalesOrders.Any())
                        {
                            var customers = context.Customers.ToList();
                            var engPackages = context.EngineeringPackages.ToList();
                            for (int i = 1; i <= 5; i++)
                            {
                                context.SalesOrders.Add(new SalesOrder
                                {
                                    OrderNumber = $"SO-100{i}",
                                    Price = i * 10000,
                                    Status = Status.Closed,
                                    CustomerID = customers[i - 1].CustomerID,
                                    EngineeringPackageID = engPackages[i - 1].EngineeringPackageID,
                                    CompletionDate = DateTime.UtcNow.AddDays(5),
                                    ActualCompletionDate = DateTime.UtcNow.AddDays(18),
                                    ExtraNotes = ""
                                });
                                
                            }
                            for (int i = 6; i <= 10; i++)
                            {
                                context.SalesOrders.Add(new SalesOrder
                                {
                                    OrderNumber = $"SO-100{i}",
                                    Price = i * 10000,
                                    Status = Status.Open,
                                    CustomerID = customers[i - 1].CustomerID,
                                    EngineeringPackageID = engPackages[i - 1].EngineeringPackageID,
                                    CompletionDate = DateTime.UtcNow.AddDays(5),
                                    ActualCompletionDate = DateTime.UtcNow.AddDays(18),
                                    ExtraNotes = ""
                                });

                            }
                            context.SaveChanges();
                        }

                        if (!context.Machines.Any())
                        {
                            string[] preOrderNotes = {
                                    "Ensure all components are available before assembly. Check for any missing parts and report immediately.",
                                    "Verify the availability of all required tools and materials before starting the assembly process.",
                                    "Confirm that all pre-order checks are completed and documented before proceeding with assembly."
                                };

                            string[] scopeNotes = {
                                    "This machine requires special handling during assembly due to its size. Ensure all safety protocols are followed.",
                                    "Pay extra attention to the alignment of components during assembly to avoid any operational issues.",
                                    "Ensure that all assembly steps are followed as per the provided guidelines to maintain quality standards."
                                };

                            string[] machineSizes = {
                                    "3' X 4'", "4' X 5'", "5' X 6'", "6' X 7'", "7' X 8'",
                                    "8' X 9'", "9' X 10'", "10' X 11'", "11' X 12'", "12' X 13'"
                                };

                            Random rnd = new Random();

                            for (int i = 1; i <= 30; i++)
                            {
                                context.Machines.Add(new Machine
                                {
                                    SerialNumber = $"SN-{1000 + i}",
                                    MachineSize = machineSizes[rnd.Next(machineSizes.Length)],
                                    MachineClass = "T",
                                    MachineSizeDesc = $"Size {i}",
                                    InternalPONumber = $"PO-{2000 + i}",
                                    AirSeal = true,
                                    Base = false,
                                    CoatingOrLining = true,
                                    Disassembly = true,
                                    Media = false,
                                    SparePartsMedia = true,
                                    PreOrderNotes = preOrderNotes[rnd.Next(preOrderNotes.Length)],
                                    ScopeNotes = scopeNotes[rnd.Next(scopeNotes.Length)],
                                    ActualAssemblyHours = (i * 2),
                                    ActualReworkHours = (i),
                                    BudgetedAssemblyHours = (i * 3)
                                });
                            }
                            context.SaveChanges();
                        }

                        if (!context.PurchaseOrders.Any())
                        {
                            var vendors = context.Vendors.ToList();
                            var salesOrders = context.SalesOrders.ToList();
                            for (int i = 1; i <= 30; i++)
                            {
                                context.PurchaseOrders.Add(new PurchaseOrder
                                {
                                    PurchaseOrderNumber = $"PO-300{i}",
                                    PODueDate = DateTime.UtcNow.AddDays(30),
                                    VendorID = vendors[i % vendors.Count].VendorID,
                                    SalesOrderID = salesOrders[i % salesOrders.Count].SalesOrderID
                                });
                            }
                            context.SaveChanges();
                        }

                        if (!context.MachineSalesOrders.Any())
                        {
                            Random rnd = new();
                            var machineSOs = new List<MachineSalesOrder>();
                            for (int salesOrderID = 1; salesOrderID <= 10; salesOrderID++)
                            {
                                for (int i = 1; i <= 3; i++)
                                {
                                    int machineID = 1 + rnd.Next(context.Machines.Count());
                                    MachineSalesOrder machineSO = new()
                                    { 
                                        MachineID = machineID, 
                                        SalesOrderID = salesOrderID 
                                    };
                                    machineSOs.Add(machineSO);
                                }

                            }
                            context.MachineSalesOrders.AddRange(machineSOs);
                            context.SaveChanges();
                        }
                        foreach (var salesOrder in context.SalesOrders)
                        {
                            var linkedMachines = context.MachineSalesOrders
                                .Include(p => p.Machine)
                                .Where(mso => mso.SalesOrderID == salesOrder.SalesOrderID)
                                .Select(mso => mso.Machine)
                                .ToList();

                            if (linkedMachines.Any())
                            {
                                foreach (var machine in linkedMachines)
                                {
                                    var ganttSchedule = new GanttSchedule
                                    {
                                        SalesOrderID = salesOrder.SalesOrderID,
                                        MachineID = machine.MachineID,
                                        EngineeringOnly = false,
                                        PreOrdersExpected = null,
                                        ReadinessToShipExpected = null,
                                        PromiseDate = DateTime.Today,
                                        DeadlineDate = null,
                                        NCR = "",
                                    };
                                    context.GanttSchedules.Add(ganttSchedule);
                                }
                            }
                            else
                            {
                                var ganttSchedule = new GanttSchedule
                                {
                                    SalesOrderID = salesOrder.SalesOrderID,
                                    EngineeringOnly = true,
                                    PreOrdersExpected = null,
                                    ReadinessToShipExpected = null,
                                    PromiseDate = DateTime.Today,
                                    DeadlineDate = null,
                                    NCR = ""
                                };
                                context.GanttSchedules.Add(ganttSchedule);
                            }

                            
                        }
                        context.SaveChanges();

                        if (!context.KickoffMeetings.Any())
                        {
                            var gantts = context.GanttSchedules.ToList();

                            if (!gantts.Any())
                            {
                                Console.WriteLine(" There is any gantt schedules on the database");
                                return;
                            }

                            List<KickoffMeeting> kickoffMeetings = new();
                            Random rnd = new();

                            foreach (var gantt in gantts)
                            {


                                for (int i = 1; i <= 3; i++)
                                {
                                    kickoffMeetings.Add(new KickoffMeeting
                                    {
                                        GanttID = gantt.GanttID,
                                        MeetingSummary = $"Kickoff Meeting {i} for Gantt {gantt.GanttID}",
                                        MeetingDate = new DateOnly(2024, rnd.Next(1, 12), rnd.Next(1, 28))
                                    });
                                }
                            }

                            context.KickoffMeetings.AddRange(kickoffMeetings);
                            context.SaveChanges();

                        }
                        else
                        {
                            Console.WriteLine("There are already kom on the database");
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.GetBaseException().Message);
                    }
                }
            }
        }
    }
}
