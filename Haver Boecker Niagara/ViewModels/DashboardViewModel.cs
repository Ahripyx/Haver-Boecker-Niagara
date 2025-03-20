namespace Haver_Boecker_Niagara.ViewModels
{
    //Step 1 create the viewModel for the charts and the landing page
    public class DashboardViewModel
    {
        public int TotalCustomers { get; set; }
        public int ConfirmedSalesOrders { get; set; }
        public int PendingSalesOrders { get; set; }
        public int TotalVendors {  get; set; }

        public int TotalSalesOrders {  get; set; }
        public int TotalPurcharseOrders { get; set; }

        public int TotalMachines { get; set; }
        public string ? UserRole { get; set; }
        public Dictionary<string, int> CustomerCountry {  get; set; } = new Dictionary<string, int>();
        public List<int> CustomerData { get; set; } = new List<int>();


    }
}
