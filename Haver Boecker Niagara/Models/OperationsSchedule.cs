using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class OperationsSchedule
    {
        public int OperationsID { get; set; }
        [DisplayName("Sales Order")]
        public string SalesOrder { get; set; }
        public int CustomerID { get; set; }
        public int VendorID { get; set; }
        [DisplayName("Machine Description")]
        public string MachineDescription { get; set; }
        [DisplayName("Serial №")]
        public string SerialNumber { get; set; }
        [DisplayName("Package Release")]
        public DateTime? PackageReleaseDate { get; set; }
        [DisplayName("PO №")]
        public string PurchaseOrderNumber { get; set; }
        [DisplayName("Purchase Order Due Date")]

        public DateTime? PODueDate { get; set; }
        [DisplayName("Purchase Order Delivery Date")]

        public DateTime? DeliveryDate { get; set; }
        public string Media { get; set; }
        [DisplayName("Spare Parts")]

        public string SparePartsMedia { get; set; }
        public string Base { get; set; }
        [DisplayName("Air Seal")]

        public string AirSeal { get; set; }
        [DisplayName("Coating / Lining")]

        public string CoatingOrLining { get; set; }
        public string Disassembly { get; set; }
        [DisplayName("Comments/Notes")]

        public string Notes { get; set; }

        public Customer Customer { get; set; }
        public Vendor Vendor { get; set; }
    }
}
