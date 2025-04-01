using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class AccountVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name ="User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [RegularExpression(@"^(?=.*[^a-zA-Z0-9]).{6,}$", ErrorMessage = "La contraseña debe contener al menos un carácter especial.")]
        public string Password { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
