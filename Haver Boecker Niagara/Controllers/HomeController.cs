using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Haver_Boecker_Niagara.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HaverContext _context;
        public HomeController(ILogger<HomeController> logger, HaverContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Step 2 you need to add the values into the variables in dashBoardViewModel
        public IActionResult Index()
        {
            var TotalCustomers = _context.Customers.Count();
            var confirmedSalesOrders = _context.SalesOrders.Count(g => g.Status == Status.Closed);
            var pendingSalesOrders = _context.SalesOrders.Count(g => g.Status == Status.Open);
            var  totalVendors = _context.Vendors.Count();
            var totalMachines = _context.Machines.Count();
            
            var customerData = _context.Customers
                .GroupBy(g => g.CreatedAt.Month)
                .Select(g => g.Count())
                .ToList();

            var customerCountry = _context.Customers
                .GroupBy(g => g.Country)
                .ToDictionary(g => g.Key, g => g.Count());
            var model = new DashboardViewModel
            {
                TotalCustomers = TotalCustomers,
                ConfirmedSalesOrders = confirmedSalesOrders,
                PendingSalesOrders = pendingSalesOrders,
                TotalVendors = totalVendors,
                TotalMachines = totalMachines,
                CustomerData = customerData,
                CustomerCountry = customerCountry
            };
            return View(model);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult _Login()
        {
            return View();
        }
    }
}
