using Azure;

namespace Haver_Boecker_Niagara.Models
{
    public class EngineeringPackage
    {
        public int EngineeringPackageID { get; set; }
        
        public DateTime? PackageReleaseDate { get; set; }
        public DateTime? ApprovalDrawingDate { get; set; }
        // will be available document upload
        public DateTime? ActualPackageReleaseDate { get; set; }
        public DateTime? ActualApprovalDrawingDate { get; set; }

        public ICollection<Engineer> Engineers { get; set; } = new HashSet<Engineer>();
        public SalesOrder SalesOrder { get; set; }


    }
}
