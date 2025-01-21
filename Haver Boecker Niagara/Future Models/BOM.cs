namespace Haver_Boecker_Niagara.Models
{
    public class BOM
    {
        public int BOM_ID { get; set; }
        public int OrderID { get; set; }
        public DateTime BOM_Date { get; set; }
        public string BOM_Details { get; set; }

        public GanttSchedule GanttSchedule { get; set; }
    }
}
