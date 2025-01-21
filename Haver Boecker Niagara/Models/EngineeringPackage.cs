namespace Haver_Boecker_Niagara.Models
{
    public class PackageRelease
    {
        public int DrawingID { get; set; }
        
        public int EngineerID { get; set; }

        public DateTime? PackageReleaseDate { get; set; } // kickoff meeting
        public DateTime? ApprovalDrawingDate { get; set; } // "A" in excel


        // this model will later allow document upload (approval drawing)

    }
}
