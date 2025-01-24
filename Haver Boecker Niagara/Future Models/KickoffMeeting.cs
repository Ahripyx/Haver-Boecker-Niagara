namespace Haver_Boecker_Niagara.Models
{
    public class KickoffMeeting
    {
        public int MeetingID { get; set; }
        public int OrderID { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Participants { get; set; }
        public string KeyDiscussions { get; set; }
        public string Status { get; set; }

        public GanttSchedule GanttSchedule { get; set; }
    }
}
