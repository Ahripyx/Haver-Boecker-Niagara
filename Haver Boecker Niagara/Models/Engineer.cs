namespace Haver_Boecker_Niagara.Models
{
    public class Engineer
    {
        public int EngineerID { get; set; }
        public string Name { get; set; }
        public string Initials { get; set; }
        public string Email { get; set; }

        public ICollection<EngineeringPackage> EngineeringPackages { get; set; } = new HashSet<EngineeringPackage>();

    }
}
