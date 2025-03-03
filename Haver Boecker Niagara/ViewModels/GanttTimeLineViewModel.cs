using Haver_Boecker_Niagara.Models;
using System.ComponentModel;

namespace Haver_Boecker_Niagara.ViewModels
{
    public class GanttTimeLineViewModel
    {
        public int GanttID { get; set; }
        public string OrderNumber { get; set; }

        public string? LatestMilestone { get; set; }
        public string CustomerName { get; set; }
        public List<string> EngineerInitials { get; set; }
        public DateTime? PreOrdersExpected { get; set; }
        public DateTime? ReadinessToShipExpected { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public bool EngineeringOnly { get; set; }
        public string MachineDescription { get; set; }
        public string Media { get; set; }
        public string SparePartsMedia { get; set; }
        public List<MilestoneViewModel> Milestones { get; set; }
    }
    public class MilestoneViewModel
    {

        public int MilestoneID { get; set; }

        [DisplayName("Task Name")]
        public string Name { get; set; }

        public int KickOfMeetingID { get; set; }
        public KickoffMeeting? KickoffMeeting { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Actual Completion Date")]
        public DateTime? ActualCompletionDate { get; set; }

        [DisplayName("Task Status")]
        public string Status { get; set; }
    }
}

