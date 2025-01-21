namespace Haver_Boecker_Niagara.Models
{
    public class EngineeringPackage
    {
        public int EngineeringPackageID { get; set; }
        
        public int EngineerID { get; set; }

        public DateTime? PackageReleaseDate { get; set; } // kickoff meeting
        public DateTime? ApprovalDrawingDate { get; set; } // "A" in excel


        // this model will later allow document upload (approval drawing)

        public Engineer Engineer { get; set; }

        public ICollection<OperationsSchedule> OperationsSchedules { get; set; } = new HashSet<OperationsSchedule>();


    }
}
