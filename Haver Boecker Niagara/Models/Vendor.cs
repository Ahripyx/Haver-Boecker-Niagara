using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class Vendor
    {
        public int VendorID { get; set; }


        [DisplayName("Vendor Name")]
        public string Name { get; set; }

        [DisplayName("Contact")]
        public string? ContactPerson { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [DisplayName("Email Address")]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string Email { get; set; }

        [DisplayName("Street Address")]
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        [DisplayName("Date Created")]
        public DateTime CreatedAt { get; set; }
        [DisplayName("Last Updated")]
        public DateTime UpdatedAt { get; set; }

        public ICollection<OperationsVendors> OperationsVendors { get; set; }
    }
}