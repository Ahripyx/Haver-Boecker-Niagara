using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        [Authorize(Roles = "admin, procurement,engineering,sales, production, pic, read only")]
        public IActionResult Index()
        {
            var TotalCustomers = _context.Customers.Count();
            var TotalSalesOrders = _context.SalesOrders.Count();
            var confirmedSalesOrders = _context.SalesOrders.Where(g => g.Status == Status.Closed).Count();
            var totalSalesOrders = _context.SalesOrders.Count();
            var pendingSalesOrders = _context.SalesOrders.Where(g => g.Status == Status.Open).Count();

            string userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "User";

            var vendorsByMonth = _context.Vendors
                .GroupBy(g => g.CreatedAt.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .OrderBy(g => g.Month)
                .ToList();
            var totalVendors = _context.Vendors.Count();
            var vendorMonths = new int[12]; 
            foreach (var i in vendorsByMonth)
            {
                vendorMonths[i.Month - 1] = i.Count;
            }
            ViewBag.vendorsByMonth = vendorMonths;

            var machineBySize = _context.Machines
                .GroupBy(g => g.MachineSize)
                .Select(g => new { Size = g.Key, Count = g.Count() })
                .OrderBy(g => g.Size)
                .ToList();
            var totalMachines = _context.Machines.Count();
            var machineSizes = new Dictionary<string, int>();
            foreach (var i in machineBySize)
            {
                machineSizes[i.Size] = i.Count;
            }
            ViewBag.MachineBySizes = machineSizes;
            var customerData = _context.Customers
                .GroupBy(g => g.CreatedAt.Month)
                .Select(g => g.Count())
                .ToList();
            var customerCountry = _context.Customers
                .GroupBy(g => g.Country ?? "No country")
                .ToDictionary(g => g.Key, g => g.Count());
            var model = new DashboardViewModel
            {
                TotalCustomers = TotalCustomers,

                ConfirmedSalesOrders = confirmedSalesOrders,
                PendingSalesOrders = pendingSalesOrders,
                TotalVendors = totalVendors,
                TotalMachines = totalMachines,
                TotalSalesOrders = TotalSalesOrders,
                CustomerData = customerData,
                CustomerCountry = customerCountry,
                UserRole = userRole
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