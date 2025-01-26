using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// ✔

namespace Haver_Boecker_Niagara.Models
{
    public class Machine
    {
        public int MachineID { get; set; }

        [Required]
        [DisplayName("Serial #")]
        [StringLength(50, ErrorMessage = "Serial number cannot exceed 50 characters.")]
        public string SerialNumber { get; set; }

        [Required]
        [DisplayName("PO #")]
        [StringLength(50, ErrorMessage = "Internal PO number cannot exceed 50 characters.")]
        public string InternalPONumber { get; set; }

        [DisplayName("Size")]
        public int MachineSize { get; set; }

        [Required]
        [DisplayName("Class")]
        [StringLength(50, ErrorMessage = "Machine class cannot exceed 50 characters.")]
        public string MachineClass { get; set; }

        [DisplayName("Description")]
        public string MachineSizeDesc { get; set; }

        [Required]
        [DisplayName("Order #")]
        public int SalesOrderID { get; set; }

        [DisplayName("Order #")]
        public SalesOrder? SalesOrder { get; set; }
    }
}