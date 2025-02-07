
namespace Haver_Boecker_Niagara.Models
{
    public class MachineSalesOrder
    {
        public int MachineSalesOrderID { get; set; }

        public int MachineID { get; set; }

        public int SalesOrderID { get; set; }

        public Machine Machine { get; set; } 
        public SalesOrder SalesOrder { get; set; } 
    }
}
