namespace Haver_Boecker_Niagara.Models
{
    public class OperationsVendors
    {

        public int OperationsVendorsID { get; set; }

        public int OperationsID { get; set; }
        public int VendorsID { get; set; }

        public OperationsSchedule OperationsSchedule { get; set; }
        public Vendor Vendor { get; set; }
    }
}
