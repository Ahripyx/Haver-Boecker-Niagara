using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class Milestone
    {
        public int MilestoneID { get; set; }

        [DisplayName("Task Name")]
        public string Name { get; set; } //dropdown with all possible values as Approwed drwngr recieved and etc for everything 
        public int GanttScheduleID { get; set; }
        public GanttSchedule? GanttSchedule { get; set; } 

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Actual Completion Date")]
        public DateTime? ActualCompletionDate { get; set; }

        [DisplayName("Task Status")]
        public string Status { get; set; } //need to be dropdown list for choice

        public ICollection<ProgressLog>? Dependencies { get; set; }

    }
}
