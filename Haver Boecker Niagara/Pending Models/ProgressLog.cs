namespace Haver_Boecker_Niagara.Models
{
    public class ProgressLog
    {
        public int LogID { get; set; }
        public int PredecessorTaskID { get; set; }
        public KickoffMeeting? PredecessorTask { get; set; } // the task that must be completed first

        public int SuccessorTaskID { get; set; }
        public KickoffMeeting? SuccessorTask { get; set; } // the task that depends on the first task

        public string DependencyType { get; set; } //need to be as dropdown list with re4lational descriptions term
    }
}