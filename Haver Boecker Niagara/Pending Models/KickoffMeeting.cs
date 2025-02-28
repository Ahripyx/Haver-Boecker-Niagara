using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class KickoffMeeting
    {
        public int MeetingID { get; set; }
        public int SalesOrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }
    
        public string OrderStatus { get; set; } //dropdown for our future showing it in the workflow of sales order 

        public ICollection<Milestone>? Milestones { get; set; }

        //field that will be connected to the sales order from which we are gonna take info for gantt
    }
}
