using Haver_Boecker_Niagara.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Haver_Boecker_Niagara.Models
{

    public class SalesOrder
    {
        public int SalesOrderID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int? CustomerID { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        [Display(Name = "Order №")]
        [StringLength(50, ErrorMessage = "Order number cannot exceed 50 characters.")]
        public string OrderNumber { get; set; }

        [DisplayName("Est. Completion Date")]
        public DateTime? CompletionDate { get; set; }

        [NotMapped]
        public string EstimatedCompletionSummary => CompletionDate?.ToShortDateString() ?? "N/A";

        [DisplayName("Actual Release Date")]
        public DateTime? ActualCompletionDate { get; set; }

        [NotMapped]
        public string ActualCompletionSummary => ActualCompletionDate?.ToShortDateString() ?? "N/A";

        [DisplayName("Comments/Notes")]
        public string? ExtraNotes { get; set; }

        [DisplayName("Engineering Package")]
        public int? EngineeringPackageID { get; set; }

        public EngineeringPackage? EngineeringPackage { get; set; }
        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; } = new HashSet<PurchaseOrder>();

        public ICollection<Machine> Machines { get; set; } = new HashSet<Machine>();
        public ICollection<GanttSchedule>? GanttSchedules { get; set; } = new HashSet<GanttSchedule>();

    }
}