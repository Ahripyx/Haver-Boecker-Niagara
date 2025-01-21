// lookup table for PO#

using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; }

        public int OperationsID { get; set; }

        public string PurchaseOrderNumber { get; set; }


        [DisplayName("Purchase Order Due Date")]
        public DateTime? PODueDate { get; set; } 

        public int VendorID { get; set; }

        public Vendor Vendor { get; set; }
        public OperationsSchedule OperationsSchedule { get; set; }



    }
}
