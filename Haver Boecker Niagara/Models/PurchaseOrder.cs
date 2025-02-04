// lookup table for PO#

using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Haver_Boecker_Niagara.Models
{
    public class PurchaseOrder
    {
        public int PurchaseOrderID { get; set; }

        public string PurchaseOrderNumber { get; set; }


        [DisplayName("Purchase Order Due Date")]
        public DateTime? PODueDate { get; set; }

        [NotMapped]
        public string PODueDateSummary
        {
            get
            {
                if (PODueDate != null)
                {
                    return PODueDate!.Value.ToShortDateString();
                }
                return "N/A";
            }
        }
        [DisplayName("Purchase Order Actual Due Date")]
        public DateTime? POActualDueDate { get; set; }

        [NotMapped]
        public string POActualDueDateSummary
        {
            get
            {
                if (PODueDate != null)
                {
                    return PODueDate!.Value.ToShortDateString();
                }
                return "N/A";
            }
        }
        public int? VendorID { get; set; }

        public Vendor?  Vendor { get; set; }

        public int? SalesOrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }

    }
}
