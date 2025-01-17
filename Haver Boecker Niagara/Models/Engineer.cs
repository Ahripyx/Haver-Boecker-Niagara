namespace Haver_Boecker_Niagara.Models
{
    public class Engineer
    {
        public int EngineerID { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }

        public ICollection<OperationsSchedule> OperationsSchedules { get; set; }
        public ICollection<GanttSchedule> GanttSchedules { get; set; }
    }
}
