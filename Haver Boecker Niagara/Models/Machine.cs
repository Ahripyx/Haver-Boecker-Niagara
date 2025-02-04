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


        [DisplayName("Status")]
        public bool NamePlateStatus { get; set; } = false;

        [DisplayName("Size")]
        public int MachineSize { get; set; }

        [Required]
        [DisplayName("Class")]
        [StringLength(50, ErrorMessage = "Machine class cannot exceed 50 characters.")]
        public string MachineClass { get; set; }

        [DisplayName("Description")]
        public string MachineSizeDesc { get; set; }

        [DisplayName("Machine Description")]
        public string MachineDescription => $"{MachineClass} {MachineSize} {MachineSizeDesc}";
        public bool Media { get; set; } = false;

        [DisplayName("Spare Parts / Media")]
        public bool SparePartsMedia { get; set; } = false;

        public bool Base { get; set; } = false;

        [DisplayName("Air Seal")]
        public bool AirSeal { get; set; } = false;

        [DisplayName("Coating / Lining")]
        public bool CoatingOrLining { get; set; } = false;

        public bool Disassembly { get; set; } = false;

        [DisplayName("Pre-Order")]
        public string? PreOrderNotes { get; set; }

        [DisplayName("Scope")]
        public string? ScopeNotes { get; set; }

        [DisplayName("Actual Assembly Hours")]
        public string? ActualAssemblyHours { get; set; }

        [DisplayName("Actual Rework Hours")]
        public string? ActualReworkHours { get; set; }

        [DisplayName("Budgeted Assembly Hours")]
        public string? BudgetedAssemblyHours { get; set; }


        [Required]
        [DisplayName("Order #")]
        public int SalesOrderID { get; set; }

        [DisplayName("Order #")]

        public SalesOrder? SalesOrder { get; set; } // ? is important
    }
}