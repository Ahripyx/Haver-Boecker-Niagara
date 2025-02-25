namespace Haver_Boecker_Niagara.Models
{
    public class GanttSchedule
    {
        public int GanttID { get; set; }
        public int SalesOrderID { get; set; }
        public SalesOrder? SalesOrder { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public bool NCR {  get; set; }

        public Customer Customer { get; set; }
        public Engineer Engineer { get; set; }
    }

}
