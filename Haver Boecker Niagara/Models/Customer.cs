using System.ComponentModel;

namespace Haver_Boecker_Niagara.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [DisplayName("Customer Name")]
        public string Name { get; set; }

        [DisplayName("Contact")]
        public string ContactPerson { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        [DisplayName("Date Created")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Last Updated")]
        public DateTime UpdatedAt { get; set; }
        public ICollection<OperationsSchedule> OperationsSchedules { get; set; }
        public ICollection<GanttSchedule> GanttSchedules { get; set; }
    }
}