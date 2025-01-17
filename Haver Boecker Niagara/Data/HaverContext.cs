using Haver_Boecker_Niagara.Models;
using Microsoft.EntityFrameworkCore;


namespace Haver_Boecker_Niagara.Data;


public class HaverContext : DbContext
{
    public HaverContext(DbContextOptions<HaverContext> options) : base(options) { }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Engineer> Engineers { get; set; }
    public DbSet<OperationsSchedule> OperationsSchedules { get; set; }
    public DbSet<ProcurementLog> ProcurementLogs { get; set; }
    public DbSet<QualityLog> QualityLogs { get; set; }
    public DbSet<AssemblyLog> AssemblyLogs { get; set; }
    public DbSet<GanttSchedule> GanttSchedules { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<KickoffMeeting> KickoffMeetings { get; set; }
    public DbSet<ApprovalDrawing> ApprovalDrawings { get; set; }
    public DbSet<ProgressLog> ProgressLogs { get; set; }
    public DbSet<BOM> BOMs { get; set; }
    public DbSet<NCR> NCRs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>()
            .HasKey(c => c.CustomerID); 

        modelBuilder.Entity<Vendor>()
            .HasKey(v => v.VendorID);

        modelBuilder.Entity<Engineer>()
            .HasKey(e => e.EngineerID);

        modelBuilder.Entity<OperationsSchedule>()
            .HasKey(o => o.OperarionsID);  

        modelBuilder.Entity<ProcurementLog>()
            .HasKey(p => p.ProcurementLogID); 

        modelBuilder.Entity<QualityLog>()
            .HasKey(q => q.QualityLogID); 

        modelBuilder.Entity<AssemblyLog>()
            .HasKey(a => a.AssemblyLogID); 

        modelBuilder.Entity<GanttSchedule>()
            .HasKey(g => g.GanttID);  

        modelBuilder.Entity<Milestone>()
            .HasKey(m => m.MilestoneID);  

        modelBuilder.Entity<KickoffMeeting>()
            .HasKey(k => k.MeetingID); 

        modelBuilder.Entity<ApprovalDrawing>()
            .HasKey(a => a.DrawingID); 

        modelBuilder.Entity<ProgressLog>()
            .HasKey(p => p.LogID);

        modelBuilder.Entity<BOM>()
            .HasKey(b => b.BOM_ID); 

        modelBuilder.Entity<NCR>()
            .HasKey(n => n.NCR_ID); 


        modelBuilder.Entity<OperationsSchedule>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OperationsSchedule>()
            .HasOne(o => o.Engineer)
            .WithMany()
            .HasForeignKey(o => o.EngineerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProcurementLog>()
            .HasOne(p => p.Vendor)
            .WithMany()
            .HasForeignKey(p => p.VendorID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProcurementLog>()
            .HasOne(p => p.OperationsSchedule)
            .WithMany()
            .HasForeignKey(p => p.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<QualityLog>()
            .HasOne(q => q.OperationsSchedule)
            .WithMany()
            .HasForeignKey(q => q.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssemblyLog>()
            .HasOne(a => a.OperationsSchedule)
            .WithMany()
            .HasForeignKey(a => a.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GanttSchedule>()
            .HasOne(g => g.Customer)
            .WithMany()
            .HasForeignKey(g => g.CustomerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<GanttSchedule>()
            .HasOne(g => g.Engineer)
            .WithMany()
            .HasForeignKey(g => g.EngineerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Milestone>()
            .HasOne(m => m.GanttSchedule)
            .WithMany()
            .HasForeignKey(m => m.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<KickoffMeeting>()
            .HasOne(k => k.GanttSchedule)
            .WithMany()
            .HasForeignKey(k => k.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApprovalDrawing>()
            .HasOne(a => a.GanttSchedule)
            .WithMany()
            .HasForeignKey(a => a.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProgressLog>()
            .HasOne(p => p.GanttSchedule)
            .WithMany()
            .HasForeignKey(p => p.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BOM>()
            .HasOne(b => b.GanttSchedule)
            .WithMany()
            .HasForeignKey(b => b.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<NCR>()
            .HasOne(n => n.GanttSchedule)
            .WithMany()
            .HasForeignKey(n => n.OrderID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

