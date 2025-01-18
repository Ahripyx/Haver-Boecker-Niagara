using Haver_Boecker_Niagara.Models;
using Microsoft.EntityFrameworkCore;

namespace Haver_Boecker_Niagara.Data
{
    public class HaverInitializer
    {
        public static void Initialize(HaverContext context)
        {
            context.Database.EnsureCreated();
            if (context.Customers.Any() || context.Vendors.Any() || context.OperationsSchedules.Any())
            {
                return; 
            }

            var customers = new List<Customer>();
            for (int i = 1; i <= 10; i++)
            {
                customers.Add(new Customer
                {
                    Name = $"Customer {i}",
                    ContactPerson = $"Contact Person {i}",
                    PhoneNumber = $"123-456-789{i}",
                    Email = $"customer{i}@example.com",
                    Address = $"123 Street {i}",
                    City = $"City {i}",
                    State = "State",
                    Country = "Country",
                    PostalCode = $"12345{i}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            context.Customers.AddRange(customers);
            context.SaveChanges();

            var vendors = new List<Vendor>();
            for (int i = 1; i <= 10; i++)
            {
                vendors.Add(new Vendor
                {
                    Name = $"Vendor {i}",
                    ContactPerson = $"Vendor Contact {i}",
                    PhoneNumber = $"987-654-321{i}",
                    Email = $"vendor{i}@example.com",
                    Address = $"456 Avenue {i}",
                    City = $"Vendor City {i}",
                    State = "Vendor State",
                    Country = "Vendor Country",
                    PostalCode = $"54321{i}",
                    Rating = i % 5 + 1, 
                    Description = $"Description for Vendor {i}",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            context.Vendors.AddRange(vendors);
            context.SaveChanges();

            var operationsSchedules = new List<OperationsSchedule>();
            foreach (var customer in customers)
            {
                for (int i = 1; i <= 3; i++)
                {
                    var vendor = vendors[(i - 1) % vendors.Count];
                    operationsSchedules.Add(new OperationsSchedule
                    {
                        SalesOrder = $"SO-{customer.Name}-{i}",
                        CustomerName = customer.Name,
                        MachineDescription = $"Machine Type {i} for {customer.Name}",
                        SerialNumber = $"SN-{customer.Name}-{i:000}",
                        PackageReleaseDate = DateTime.UtcNow.AddDays(i * 10),
                        VendorName = vendor.Name,
                        PurchaseOrderNumber = $"PO-{vendor.Name}-{i}",
                        PODueDate = DateTime.UtcNow.AddDays(i * 5),
                        DeliveryDate = DateTime.UtcNow.AddDays(i * 15),
                        Media = $"Media Type {i}",
                        SparePartsMedia = $"Spare Parts {i}",
                        Base = $"Base Type {i}",
                        AirSeal = $"Air Seal Type {i}",
                        CoatingOrLining = $"Coating Type {i}",
                        Disassembly = $"Disassembly Info {i}",
                        Notes = $"Notes for Order {i} of {customer.Name}",
                        Customer = customer,
                        Vendor = vendor
                    });
                }
            }
            context.OperationsSchedules.AddRange(operationsSchedules);
            context.SaveChanges();
        }
    }
}
