using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class SalesOrder
    {
        public int SalesOrderID { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public Customer Customer { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Order number cannot exceed 50 characters.")]
        public string OrderNumber { get; set; }
        public bool Media { get; set; } = false;

        [DisplayName("Spare Parts / Media")]
        public bool SparePartsMedia { get; set; } = false;

        public bool Base { get; set; } = false;

        [DisplayName("Air Seal")]
        public bool AirSeal { get; set; } = false;

        [DisplayName("Coating / Lining")]
        public bool CoatingOrLining { get; set; } = false;

        public bool Disassembly { get; set; } = false;


        [DisplayName("Engineering Package")]
        public int? EngineeringPackageID { get; set; }

        public EngineeringPackage? EngineeringPackage { get; set; }
        public ICollection<PurchaseOrder>? PurchaseOrders { get; set; } = new HashSet<PurchaseOrder>();

        public ICollection<OperationsSchedule>? OperationsSchedules { get; set; } = new HashSet<OperationsSchedule>();  // This property is already here

        public ICollection<Machine>? Machines { get; set; } = new HashSet<Machine>();
    }

}
