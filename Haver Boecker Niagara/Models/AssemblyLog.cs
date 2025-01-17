namespace Haver_Boecker_Niagara.Models
{
    public class AssemblyLog
    {
        public int AssemblyLogID { get; set; }
        public int OrderID { get; set; }
        public DateTime? AssemblyStartDate { get; set; }
        public DateTime? AssemblyEndDate { get; set; }
        public int? ActualAssemblyHours { get; set; }
        public int? ReworkHours { get; set; }
        public string Status { get; set; }

        public OperationsSchedule OperationsSchedule { get; set; }
    }
}
