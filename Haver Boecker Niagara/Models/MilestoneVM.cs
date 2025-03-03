using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class MilestoneVM
    {
        public int MilestoneID { get; set; }

        public int KickOfMeetingID { get; set; }

        [DisplayName("Task Name")]
        [Required]
        public Task Name { get; set; }

        [DisplayName("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Actual Completion Date")]
        public DateTime? ActualCompletionDate { get; set; }

        [DisplayName("Task Status")]
        [Required]
        public string Status { get; set; }

        public static MilestoneVM FromMilestone(Milestone milestone, int kickOfMeetingID)
        {
            return new MilestoneVM
            {
                MilestoneID = milestone.MilestoneID,
                KickOfMeetingID = kickOfMeetingID,
                Name = milestone.Name,
                StartDate = milestone.StartDate,
                EndDate = milestone.EndDate,
                ActualCompletionDate = milestone.ActualCompletionDate,
                Status = milestone.Status
            };
        }
    }
}