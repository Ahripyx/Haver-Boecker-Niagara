using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class Machine
    {
        public int MachineID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Serial number cannot exceed 50 characters.")]
        public string SerialNumber { get; set; }

        [DisplayName("Machine Size")]
        public int MachineSize { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Machine class cannot exceed 50 characters.")]
        public string MachineClass { get; set; }

        [DisplayName("Machine Size Description")]
        public string MachineSizeDesc { get; set; }

        public bool Media { get; set; } = false;

        [DisplayName("Spare Parts Media")]
        public bool SparePartsMedia { get; set; } = false;

        public bool Base { get; set; } = false;

        [DisplayName("Air Seal")]
        public bool AirSeal { get; set; } = false;

        [DisplayName("Coating / Lining")]
        public bool CoatingOrLining { get; set; } = false;

        public bool Disassembly { get; set; } = false;

        [Required]
        public int SalesOrderID { get; set; }

        public SalesOrder SalesOrder { get; set; }
    }
}
