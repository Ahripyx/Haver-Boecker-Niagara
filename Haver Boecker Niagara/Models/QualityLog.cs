namespace Haver_Boecker_Niagara.Models
{
    public class QualityLog
    {
        public int QualityLogID { get; set; }
        public int OrderID { get; set; }
        public string ProductionOrder { get; set; }
        public string QualityChecks { get; set; }
        public string IssuesFound { get; set; }
        public bool ReworkRequired { get; set; }
        public string Status { get; set; }

        public OperationsSchedule OperationsSchedule { get; set; }
    }
}
