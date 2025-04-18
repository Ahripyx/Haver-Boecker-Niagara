﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace Haver_Boecker_Niagara.Models { 

    public class MachineVM
    {
        public int MachineID { get; set; }

        public int SalesOrderID { get; set; }

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
        public string MachineSize { get; set; }

        [Required]
        [DisplayName("Class")]
        [StringLength(50, ErrorMessage = "Machine class cannot exceed 50 characters.")]
        public string MachineClass { get; set; }

        [DisplayName("Description")]
        public string MachineSizeDesc { get; set; }
       
        
        public bool Media { get; set; } = false;

        [DisplayName("Spare Parts/Media")]
        public bool SparePartsMedia { get; set; } = false;

        public bool Base { get; set; } = false;

        [DisplayName("Air Seal")]

        public bool AirSeal { get; set; } = false;

        [DisplayName("Coating/Lining")]
        public bool CoatingOrLining { get; set; } = false;
        public bool Disassembly { get; set; } = false;

        [DisplayName("Pre-Order Notes")]
        public string? PreOrderNotes { get; set; }

        [DisplayName("Scope Notes")]
        public string? ScopeNotes { get; set; }

        [DisplayName("Actual Assembly Hours")]
        public int? ActualAssemblyHours { get; set; }

        [DisplayName("Actual Rework Hours")]
        public int? ActualReworkHours { get; set; }

        [DisplayName("Budgeted Assembly Hours")]
        public int? BudgetedAssemblyHours { get; set; }

        public static MachineVM FromMachine(Machine machine, int salesOrderID)
        {
            return new MachineVM
            {
                MachineID = machine.MachineID,
                SalesOrderID = salesOrderID,
                SerialNumber = machine.SerialNumber,
                InternalPONumber = machine.InternalPONumber,
                MachineSize = machine.MachineSize,
                MachineClass = machine.MachineClass,
                MachineSizeDesc = machine.MachineSizeDesc,
                Media = machine.Media,
                SparePartsMedia = machine.SparePartsMedia,
                Base = machine.Base,
                AirSeal = machine.AirSeal,
                CoatingOrLining = machine.CoatingOrLining,
                Disassembly = machine.Disassembly
            };
        }
    }   
}
