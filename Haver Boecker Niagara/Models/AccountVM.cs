using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class AccountVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
