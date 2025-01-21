namespace Haver_Boecker_Niagara.Models
{
    public class Milestone
    {
        public int MilestoneID { get; set; }
        public int OrderID { get; set; }
        public string MilestoneName { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? ActualDate { get; set; }
        public DateTime? ProjectedDate { get; set; }
        public string Status { get; set; }
        public string SpecialNotes { get; set; }
        public DateTime? DateChanged { get; set; }

        public GanttSchedule GanttSchedule { get; set; }
    }
}
