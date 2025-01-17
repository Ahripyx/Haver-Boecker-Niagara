namespace Haver_Boecker_Niagara.Models
{
    public class ApprovalDrawing
    {
        public int DrawingID { get; set; }
        public int OrderID { get; set; }
        public DateTime IssuedDate { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string Status { get; set; }

        public GanttSchedule GanttSchedule { get; set; }
    }
}
