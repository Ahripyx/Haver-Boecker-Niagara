using System.ComponentModel.DataAnnotations;

namespace Haver_Boecker_Niagara.Models
{
    public class Engineer
    {
        public int EngineerID { get; set; }

        [Display(Name= "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Name
        {
            get => $"{FirstName} {LastName}".Trim();
        }

        public string Initials
        {
            get => $"{FirstName?[0]}{LastName?[0]}".ToUpper();
        }

        public string Email { get; set; }

        public ICollection<EngineeringPackage>? EngineeringPackages { get; set; } = new HashSet<EngineeringPackage>();
    }
}
