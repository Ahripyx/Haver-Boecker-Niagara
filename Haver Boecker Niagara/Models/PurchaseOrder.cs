// lookup table for PO#

namespace Haver_Boecker_Niagara.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; }

        public int OperationsID { get; set; }

        public string PurchaseOrderNumber { get; set; }
        
        public DateTime? PODueDate { get; set; } // ask if they all should be shared or if they are independent

    }
}
