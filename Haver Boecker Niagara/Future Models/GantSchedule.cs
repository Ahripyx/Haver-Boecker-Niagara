using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class GanttSchedule
    {
        public int GanttID { get; set; }
        public int SalesOrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }
        public bool EngineeringOnly { get; set; }

        public string? LatestMilestone { get; set; }
        public int? MachineID { get; set; }

        [DisplayName("Pre-Orders Expected")]
        public DateTime? PreOrdersExpected { get; set; }

        [DisplayName("Readiness to Ship Expected")]
        public DateTime? ReadinessToShipExpected { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public string? NCR { get; set; }
        public ICollection<KickoffMeeting>? KickoffMeetings { get; set; }


        //controller for gantt is taking all info from excel from Sales order and  on edit u can add new sales order without Machines and then field Engineering only will be our flag
    }

}