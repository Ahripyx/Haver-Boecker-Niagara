﻿using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class SalesOrder
    {
        public int SalesOrderID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Order number cannot exceed 50 characters.")]
        public string OrderNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string Status { get; set; }

        [Required]
        public int CustomerID { get; set; }

        public Customer Customer { get; set; }

        public ICollection<OperationsSchedule> OperationsSchedules { get; set; } = new HashSet<OperationsSchedule>();

        public ICollection<Machine> Machines { get; set; } = new HashSet<Machine>();
    }
}
