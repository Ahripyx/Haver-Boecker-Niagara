namespace Haver_Boecker_Niagara.ViewModels
{
    public class OperationsScheduleViewModel
    {
        public int Id { get; set; }
        public string? SalesOrder { get; set; }
        public string? CustomerName { get; set; }
        public List<string>? Machines { get; set; }
        public List<string> SerialAndIPO { get; set; }
        public PackageReleaseViewModel? PackageRelease { get; set; }
        public List<string>? Vendors { get; set; }
        public List<string>? PurchaseOrders { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public AdditionalDetailsViewModel? AdditionalDetails { get; set; }
        public NotesViewModel? Notes { get; set; }
    }

    public class PackageReleaseViewModel
    {
        public List<string>? Engineers { get; set; }
        public string? EstimatedReleaseSummary { get; set; }
        public string? EstimatedApprovalSummary { get; set; }
    }

    public class AdditionalDetailsViewModel
    {
        public string Media { get; set; }
        public string SparePartsMedia { get; set; }
        public string Base { get; set; }
        public string AirSeal { get; set; }
        public string CoatingOrLining { get; set; }
        public string Disassembly { get; set; }
    }

    public class NotesViewModel
    {
        public string? PreOrderNotes { get; set; }
        public string?   ScopeNotes { get; set; }
        public string? ActualAssemblyHours { get; set; }
        public string? ActualReworkHours { get; set; }
        public string? BudgetedAssemblyHours { get; set; }
        public string? NamePlateStatus { get; set; }
        public string? ExtraNotes { get; set; }
    }

}
