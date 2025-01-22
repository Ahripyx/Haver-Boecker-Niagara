using Azure;

namespace Haver_Boecker_Niagara.Models
{
    public class EngineeringPackage
    {
        public int EngineeringPackageID { get; set; }
        
        public DateTime? PackageReleaseDate { get; set; } // kickoff meeting
        public DateTime? ApprovalDrawingDate { get; set; } // "A" in excel


        // this model will later allow document upload (approval drawing)
        public ICollection<Engineer> Engineers { get; set; } = new HashSet<Engineer>();


        public ICollection<SalesOrder> SalesOrders { get; set; } = new HashSet<SalesOrder>();


    }
}
