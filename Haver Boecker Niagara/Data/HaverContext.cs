using Haver_Boecker_Niagara.Models;
using Microsoft.EntityFrameworkCore;

namespace Haver_Boecker_Niagara.Data
{
    public class HaverContext : DbContext
    {
        public HaverContext(DbContextOptions<HaverContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Engineer> Engineers { get; set; }
        public DbSet<OperationsSchedule> OperationsSchedules { get; set; }
        public DbSet<EngineeringPackage> EngineeringPackages { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerID);
            modelBuilder.Entity<Vendor>().HasKey(v => v.VendorID);
            modelBuilder.Entity<Engineer>().HasKey(e => e.EngineerID);
            modelBuilder.Entity<OperationsSchedule>().HasKey(o => o.OperationsID);
            modelBuilder.Entity<EngineeringPackage>().HasKey(ep => ep.EngineeringPackageID);
            modelBuilder.Entity<PurchaseOrder>().HasKey(po => po.PurchaseOrderID);
            modelBuilder.Entity<Machine>().HasKey(m => m.MachineID);
            modelBuilder.Entity<SalesOrder>().HasKey(so => so.SalesOrderID);

            modelBuilder.Entity<SalesOrder>()
                .HasOne(so => so.Customer)
                .WithMany(c => c.SaleOrders)
                .HasForeignKey(so => so.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Machine>()
                .HasOne(m => m.SalesOrder)
                .WithMany(so => so.Machines)
                .HasForeignKey(m => m.SalesOrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OperationsSchedule>()
                .HasOne(os => os.SalesOrder)
                .WithMany(so => so.OperationsSchedules)
                .HasForeignKey(os => os.SalesOrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalesOrder>()
                .HasOne(so => so.EngineeringPackage)
                .WithMany(p => p.SalesOrders)
                .HasForeignKey(so => so.EngineeringPackageID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Vendor)
                .WithMany(v => v.PurchaseOrders)
                .HasForeignKey(po => po.VendorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.OperationsSchedule)
                .WithMany(os => os.PurchaseOrders)
                .HasForeignKey(po => po.OperationsID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EngineeringPackage>()
                .HasMany(ep => ep.Engineers)
                .WithMany(e => e.EngineeringPackages)
                .UsingEntity<Dictionary<string, object>>(
                    "EngineeringSpeciality",
                    j => j.HasOne<Engineer>().WithMany(),
                    j => j.HasOne<EngineeringPackage>().WithMany());
        }

    }
}
