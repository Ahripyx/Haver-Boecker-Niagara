namespace Haver_Boecker_Niagara.Models
{
    public class OperationsSchedule
    {
        public int OperationsID { get; set; }
        public string SalesOrder { get; set; }
        public int CustomerID { get; set; } 
        public string CustomerName { get; set; } 
        public string MachineDescription { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? PackageReleaseDate { get; set; }
        public int VendorID { get; set; } 
        public string VendorName { get; set; } 
        public string PurchaseOrderNumber { get; set; }
        public DateTime? PODueDate { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public string Media { get; set; }
        public string SparePartsMedia { get; set; }
        public string Base { get; set; }
        public string AirSeal { get; set; }
        public string CoatingOrLining { get; set; }
        public string Disassembly { get; set; }

        public string Notes { get; set; }

        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
    }
}
