using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class KickoffMeeting
    {
        public int MeetingID { get; set; }
        public int GanttID { get; set; }
        public GanttSchedule? GanttSchedule { get; set; }

        [Required]
        [DisplayName("Meeting Summary")]
        [MaxLength(250, ErrorMessage = "Meeting Summary cannot be longer than 250 characters.")]
        public string MeetingSummary { get; set; }
        public DateOnly MeetingDate { get; set; }
        public ICollection<Milestone>? Milestones { get; set; }

        //field that will be connected to the sales order from which we are gonna take info for gantt
    }
}