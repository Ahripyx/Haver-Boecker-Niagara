namespace Haver_Boecker_Niagara.Models
{
    public class EngineeringPackageEngineer
    {
        public int EngineeringPackageEngineerID { get; set; }
        public int EngineerID { get; set; }

        public Engineer? Engineer { get; set; }

        public int EngineeringPackageID { get; set; }

        public EngineeringPackage? EngineeringPackage { get; set; }
    }
}
