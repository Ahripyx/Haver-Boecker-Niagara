namespace Haver_Boecker_Niagara.Models
{
    public class GanttSchedule
    {
        public int OrderID { get; set; }
        public string OrderNumber { get; set; }
        public int CustomerID { get; set; }
        public int EngineerID { get; set; }
        public string MachineDetails { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string Status { get; set; }

        public Customer Customer { get; set; }
        public Engineer Engineer { get; set; }
    }

}
