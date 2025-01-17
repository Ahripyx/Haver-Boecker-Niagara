namespace Haver_Boecker_Niagara.Models
{
    public class OperationsSchedule
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int EngineerID { get; set; }
        public string MachineDetails { get; set; }
        public DateTime? PODueDate { get; set; }
        public DateTime? ApprovalDrawingRelease { get; set; }
        public DateTime? PackageReleaseDate { get; set; }
        public string ITPRequirements { get; set; }
        public string PreOrderInfo { get; set; }
        public int? BudgetedAssemblyHours { get; set; }
        public int? ActualAssemblyHours { get; set; }
        public int? ReworkHours { get; set; }
        public string ProductionOrderNumber { get; set; }
        public string NameplateStatus { get; set; }
        public string PackagingStatus { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }

        public Customer Customer { get; set; }
        public Engineer Engineer { get; set; }
    }
}
