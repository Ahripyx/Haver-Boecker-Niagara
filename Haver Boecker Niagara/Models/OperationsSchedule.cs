using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class OperationsSchedule
    {
        public int OperationsID { get; set; }

        public int SalesOrderID { get; set; }

        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; } = new HashSet<PurchaseOrder>();

        [DisplayName("Delivery Date")]
        public DateTime? DeliveryDate { get; set; }


        [DisplayName("Pre-Order")]
        public string? PreOrderNotes {  get; set; }

        [DisplayName("Scope")]
        public string? ScopeNotes {  get; set; }

        [DisplayName("Actual Assembly Hours")]
        public string? ActualAssemblyHours {  get; set; }

        [DisplayName("Actual Rework Hours")]
        public string? ActualReworkHours {  get; set; }

        [DisplayName("Budgeted Assembly Hours")]
        public string? BudgetedAssemblyHours {  get; set; }

        [DisplayName("Name Plate Status")]
        public bool NamePlateStatus { get; set; } = false;

        [DisplayName("Comments/Notes")]
        public string ExtraNotes { get; set; }

        public SalesOrder SalesOrder { get; set; }

    }
}

// other notes:
// gantt schedule is unimportant for prototype 1 (they even specifically say *don't* program it, so we can mostly ignore it for now
