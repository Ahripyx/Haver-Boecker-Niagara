namespace Haver_Boecker_Niagara.Models
{
    public class NCR
    {
        public int NCR_ID { get; set; }
        public int? OrderID { get; set; }
        public string NCR_Number { get; set; }
        public string Description { get; set; }
        public DateTime? DateIssued { get; set; }
        public string ResolutionStatus { get; set; }
        public string IssuedBy { get; set; }
        public DateTime? ResolutionDate { get; set; }

        public GanttSchedule GanttSchedule { get; set; }
    }
}
