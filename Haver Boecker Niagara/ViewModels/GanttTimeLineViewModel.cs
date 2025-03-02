namespace Haver_Boecker_Niagara.ViewModels
{
    public class GanttTimeLineViewModel
    {
        public int GanttID { get; set; }
        public int SalesOrderID { get; set; }
        public bool EngineeringOnly { get; set; }
        public List<MilestoneViewModel> Milestones { get; set; } = new List<MilestoneViewModel>();
    }
    public class MilestoneViewModel
    {
        public string Name { get; set; }
        public DateTime? Date { get; set; }
    }
}
