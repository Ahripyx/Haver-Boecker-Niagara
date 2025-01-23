using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Azure;

namespace Haver_Boecker_Niagara.Models
{
    public class EngineeringPackage
    {
        public int EngineeringPackageID { get; set; }

        [DisplayName("Est. Release Date")]
        public DateTime? PackageReleaseDate { get; set; }

        [NotMapped]
        public string EstimatedReleaseSummary
        {
            get
            {
                if (PackageReleaseDate != null)
                {
                    return PackageReleaseDate!.Value.ToShortDateString();
                }
                return "N/A";
            }
        }

        [DisplayName("Est. Approval Date")]
        public DateTime? ApprovalDrawingDate { get; set; }

        [NotMapped]
        public string EstimatedApprovalSummary
        {
            get
            {
                if (ApprovalDrawingDate != null)
                {
                    return ApprovalDrawingDate!.Value.ToShortDateString();
                }
                return "N/A";
            }
        }
        

        [DisplayName("Actual Release Date")]
        public DateTime? ActualPackageReleaseDate { get; set; }


        [NotMapped]
        public string ActualReleaseSummary
        {
            get
            {
                if (ActualPackageReleaseDate != null)
                {
                    return ActualPackageReleaseDate!.Value.ToShortDateString();
                }
                return "N/A";
            }
        }

        [DisplayName("Actual Approval Date")]
        public DateTime? ActualApprovalDrawingDate { get; set; }

        [NotMapped]
        public string ActualApprovalSummary
        {
            get
            {
                if (ActualApprovalDrawingDate != null)
                {
                    return ActualApprovalDrawingDate!.Value.ToShortDateString();
                }
                return "N/A";
            }
        }

        public ICollection<Engineer> Engineers { get; set; } = new HashSet<Engineer>();

        public ICollection<SalesOrder> SalesOrders { get; set; } = new HashSet<SalesOrder>();

        // this model will later support document upload 

    }
}
