                                                                                                                                                                            using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class GanttSchedule
    {
        public int GanttID { get; set; }
        
        [DisplayName("Sales Order ID")]
        public int SalesOrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }
        
        [DisplayName("Engineering Only")]
        public bool EngineeringOnly { get; set; }
        
        [DisplayName("Latest Milestone")]
        public string? LatestMilestone { get; set; }
        public int? MachineID { get; set; }

        [DisplayName("Pre-Orders Expected")]
        public DateTime? PreOrdersExpected { get; set; }

        [DisplayName("Readiness to Deliver Expected")]
        public DateTime? ReadinessToShipExpected { get; set; }
        
        [DisplayName("Promised Date")]
        public DateTime PromiseDate { get; set; }
        
        [DisplayName("Need-by Date")]
        public DateTime? DeadlineDate { get; set; }
        public string? NCR { get; set; }
        public ICollection<KickoffMeeting>? KickoffMeetings { get; set; }


        //controller for gantt is taking all info from excel from Sales order and  on edit u can add new sales order without Machines and then field Engineering only will be our flag
    }

}