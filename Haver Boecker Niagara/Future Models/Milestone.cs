using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class Milestone
    {

        //doesnt need its own controller will be used in kick of meeting controller as in the end it ll be " + Add new Milestone based on KOM"
        public int MilestoneID { get; set; }

        [DisplayName("Task Name")]
        public string Name { get; set; } //dropdown with all possible values as Approwed drwngr recieved and etc for everything 

        //in its little view for new milestone need based on name fill this info either in Eng Package 
        public int KickOfMeetingID { get; set; }
        public KickoffMeeting? KickoffMeeting { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Actual Completion Date")]
        public DateTime? ActualCompletionDate { get; set; }

        [DisplayName("Task Status")]
        public string Status { get; set; } //need to be dropdown list for choice


    }
}