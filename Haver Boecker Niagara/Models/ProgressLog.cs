namespace Haver_Boecker_Niagara.Models
{
    public class ProgressLog
    {
        public int LogID { get; set; }
        public int OrderID { get; set; }
        public DateTime MeetingDate { get; set; }
        public string ProgressNotes { get; set; }
        public bool LateFlag { get; set; }
        public bool DoneFlag { get; set; }
        public string Status { get; set; }
        public DateTime? DateChanged { get; set; }

        public GanttSchedule GanttSchedule { get; set; }
    }
}
