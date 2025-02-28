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
        public DbSet<EngineeringPackage> EngineeringPackages { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<EngineeringPackageEngineer> EngineeringPackageEngineers { get; set; }
        public DbSet<GanttSchedule> GanttSchedules { get; set; }
        public DbSet<KickoffMeeting> KickoffMeetings { get; set; }
        public DbSet<Milestone> Milestones { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Customer>().HasKey(c => c.CustomerID);
            modelBuilder.Entity<Vendor>().HasKey(v => v.VendorID);
            modelBuilder.Entity<Engineer>().HasKey(e => e.EngineerID);
            modelBuilder.Entity<EngineeringPackage>().HasKey(ep => ep.EngineeringPackageID);
            modelBuilder.Entity<PurchaseOrder>().HasKey(po => po.PurchaseOrderID);
            modelBuilder.Entity<Machine>().HasKey(m => m.MachineID);
            modelBuilder.Entity<SalesOrder>().HasKey(so => so.SalesOrderID);
            modelBuilder.Entity<MachineSalesOrder>().HasKey(m => m.MachineSalesOrderID);
            modelBuilder.Entity<EngineeringPackageEngineer>().HasKey(epe => epe.EngineeringPackageEngineerID);
            modelBuilder.Entity<GanttSchedule>().HasKey(g => g.GanttID);
            modelBuilder.Entity<KickoffMeeting>().HasKey(k => k.MeetingID); 
            modelBuilder.Entity<Milestone>().HasKey(epe => epe.MilestoneID);

            modelBuilder.Entity<SalesOrder>()
                .HasOne(so => so.Customer)
                .WithMany(c => c.SaleOrders)
                .HasForeignKey(so => so.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SalesOrder>()
                .HasOne(so => so.EngineeringPackage)
                .WithOne(ep => ep.SalesOrder)
                .HasForeignKey<SalesOrder>(so => so.EngineeringPackageID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Vendor)
                .WithMany(v => v.PurchaseOrders)
                .HasForeignKey(po => po.VendorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.SalesOrder)
                .WithMany(so => so.PurchaseOrders)
                .HasForeignKey(po => po.SalesOrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Machine>()
                .HasMany(m => m.SalesOrders)
                .WithMany(s => s.Machines)
                .UsingEntity<MachineSalesOrder>();

            modelBuilder.Entity<EngineeringPackage>()
                .HasMany(ep => ep.Engineers)
                .WithMany(e => e.EngineeringPackages)
                .UsingEntity<EngineeringPackageEngineer>();
//
//            modelBuilder.Entity<GanttSchedule>()
//                .HasOne(g => g.SalesOrder)
//                .WithMany(so => so.GanttSchedules)
//                .HasForeignKey(g => g.SalesOrderID)
//                .OnDelete(DeleteBehavior.Restrict); 
//
            modelBuilder.Entity<KickoffMeeting>()
                .HasOne(k => k.GanttSchedule)
                .WithMany(g => g.KickoffMeetings)
                .HasForeignKey(k => k.GanttID)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<KickoffMeeting>()
                .HasMany(k => k.Milestones)
                .WithOne(m => m.KickoffMeeting)
                .HasForeignKey(m => m.KickOfMeetingID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
