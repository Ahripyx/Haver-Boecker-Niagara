namespace Haver_Boecker_Niagara.Models
{
    public class ProcurementLog
    {
        public int ProcurementLogID { get; set; }
        public int OrderID { get; set; }
        public int VendorID { get; set; }
        public string PONumber { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Status { get; set; }

        public OperationsSchedule OperationsSchedule { get; set; }
        public Vendor Vendor { get; set; }
    }
}
