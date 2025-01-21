using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class OperationsSchedule
    {
        public int OperationsID { get; set; }

        public SalesOrder PrimarySalesOrder { get; set; }

        public ICollection<SalesOrder> SalesOrders { get; set; } = new HashSet<SalesOrder>();

        public int CustomerID { get; set; } // -> i presume will have 'make new customer' modal and dropdown
        

        [DisplayName("Machine Description")]
        public string MachineDescription { get; set; }

        [DisplayName("Quantity")]
        public int MachineQuantity { get; set; } = 1;
        
        [DisplayName("Serial №")]
        public string SerialNumber { get; set; } // fine
        

        [DisplayName("Engineering Package")]
        public int? EngineeringPackageID {  get; set; }
        
        // primary vendor is bolded one in excel
        public int PrimaryVendorID { get; set; }

        public ICollection<OperationsVendors> OperationsVendors { get; set; } = new HashSet<OperationsVendors>();

        // keep track of PO#s
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new HashSet<PurchaseOrder>();


        [DisplayName("Purchase Order Due Date")]
        public DateTime? PODueDate { get; set; }


        [DisplayName("Purchase Order Delivery Date")]
        public DateTime? DeliveryDate { get; set; }


        public bool Media { get; set; } = false;


        [DisplayName("Spare Parts")]
        public bool SparePartsMedia { get; set; } = false;


        public bool Base { get; set; } = false;


        [DisplayName("Air Seal")]
        public bool AirSeal { get; set; } = false;


        [DisplayName("Coating / Lining")]
        public bool CoatingOrLining { get; set; } = false;


        public bool Disassembly { get; set; } = false;


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

        // maybe a feature to make a note 'important' -> in the excel, they highlight some text red to indicate importance
        public Vendor Vendor { get; set; }

        public Customer Customer { get; set; }

    }
}

// other notes:
// gantt schedule is unimportant for prototype 1 (they even specifically say *don't* program it, so we can mostly ignore it for now