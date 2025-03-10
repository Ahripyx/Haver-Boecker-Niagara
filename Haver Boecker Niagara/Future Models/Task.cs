using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public enum Task
    {
        [Description("Approval Drawings")]
        ApprovalDrawings,// 1st task and 4th task (5th on DNR and it need to be "Returned")

        [Description("Pre-Orders")]
        PreOrders, //2nd and 3rd task

        [Description("Engineering Package")]
        EngineeringPackage, //6th and 7th task

        [Description("Purchase Orders")]
        PurchaseOrders, // 8th on Open 9th on DNR which need to be Due and 10th on closed 
        /// <summary>
        /// Michael think about purchase because im not sure
        /// </summary>

        [Description("Quality Inspection Completed")]
        QualityInspectionCompleted, // 11th and has only option Closed

        [Description("NCRs Raised")]
        NCRsRaised,   // 12 th and also has only option closed

        [Description("Readiness to Ship")]
        ReadinessToShip //13 and 14 Open/Closed
    }
}
