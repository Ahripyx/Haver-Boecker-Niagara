using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class KickoffMeeting
    {
        public int MeetingID { get; set; }
        public int SalesOrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }
        public int EnggPackageId { get; set; }

        public EngineeringPackage? EngineeringPackage { get; set; }

        [DisplayName("Pre-Orders Expected")]
        public DateTime? PreOrdersExpected { get; set; }

        [DisplayName("Readiness to Ship Expected")]
        public DateTime? ReadinessToShipExpected { get; set; }
        

        //automatically created field that allow only edit on it inside of the details or edit of gantt
    }
}
