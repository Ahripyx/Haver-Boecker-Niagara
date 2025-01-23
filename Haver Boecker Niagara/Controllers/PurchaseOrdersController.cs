using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.Utilities;
using Haver_Boecker_Niagara.CustomControllers;

namespace Haver_Boecker_Niagara.Controllers
{
    public class PurchaseOrdersController : ElephantController
    {
        private readonly HaverContext _context;

        public PurchaseOrdersController(HaverContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchPONumber,
            string? searchVendor,
            DateTime? searchDueDate,
            int? page,
            int? pageSizeID,
            string? actionButton,
            string sortDirection = "asc",
            string sortField = "PurchaseOrderNumber")
        {
            string[] sortOptions = { "PurchaseOrderNumber", "VendorName", "PODueDate" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            var purchaseOrders = _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.OperationsSchedule)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchPONumber))
            {
                purchaseOrders = purchaseOrders.Where(p => EF.Functions.Like(p.PurchaseOrderNumber, $"%{searchPONumber}%"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchVendor))
            {
                purchaseOrders = purchaseOrders.Where(p => EF.Functions.Like(p.Vendor.Name, $"%{searchVendor}%"));
                filterCount++;
            }
            if (searchDueDate.HasValue)
            {
                purchaseOrders = purchaseOrders.Where(p => p.PODueDate == searchDueDate);
                filterCount++;
            }

            if (filterCount > 0)
            {
                ViewData["Filtering"] = "btn-danger";
                ViewData["NumberFilters"] = $"({filterCount} Filter{(filterCount > 1 ? "s" : "")} Applied)";
                ViewData["ShowFilter"] = "show";
            }

            if (!string.IsNullOrEmpty(actionButton) && sortOptions.Contains(actionButton))
            {
                page = 1;
                if (actionButton == sortField)
                {
                    sortDirection = sortDirection == "asc" ? "desc" : "asc";
                }
                sortField = actionButton;
            }

            purchaseOrders = sortField switch
            {
                "PurchaseOrderNumber" => sortDirection == "asc"
                    ? purchaseOrders.OrderBy(p => p.PurchaseOrderNumber)
                    : purchaseOrders.OrderByDescending(p => p.PurchaseOrderNumber),
                "VendorName" => sortDirection == "asc"
                    ? purchaseOrders.OrderBy(p => p.Vendor.Name)
                    : purchaseOrders.OrderByDescending(p => p.Vendor.Name),
                "PODueDate" => sortDirection == "asc"
                    ? purchaseOrders.OrderBy(p => p.PODueDate)
                    : purchaseOrders.OrderByDescending(p => p.PODueDate),
                _ => purchaseOrders.OrderBy(p => p.PurchaseOrderNumber)
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<PurchaseOrder>.CreateAsync(purchaseOrders, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: PurchaseOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.OperationsSchedule)
                .ThenInclude(os => os.SalesOrder)
                .FirstOrDefaultAsync(p => p.PurchaseOrderID == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        public IActionResult Create()
        {
            var salesOrders = _context.SalesOrders
                .Select(s => new { s.SalesOrderID, s.OrderNumber })
                .ToList();
            ViewData["OperationsID"] = new SelectList(salesOrders, "SalesOrderID", "OrderNumber");
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name");
            return View();
        }

        // POST: PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseOrderID,PurchaseOrderNumber,PODueDate,VendorID,OperationsID")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var salesOrders = _context.SalesOrders
                .Select(s => new { s.SalesOrderID, s.OrderNumber })
                .ToList();
            ViewData["OperationsID"] = new SelectList(salesOrders, "SalesOrderID", "OrderNumber", purchaseOrder.OperationsID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.OperationsSchedule)
                .ThenInclude(os => os.SalesOrder) 
                .FirstOrDefaultAsync(p => p.PurchaseOrderID == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            ViewBag.VendorID = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);

            ViewBag.OperationsID = new SelectList(
                _context.OperationsSchedules
                    .Include(os => os.SalesOrder)  
                    .Select(os => new
                    {
                        os.OperationsID,
                        OrderNumber = os.SalesOrder.OrderNumber 
                    }),
                "OperationsID", "OrderNumber", purchaseOrder.OperationsID);

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PurchaseOrderID,PurchaseOrderNumber,PODueDate,VendorID,OperationsID")] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PurchaseOrders.Any(e => e.PurchaseOrderID == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var salesOrders = _context.SalesOrders
                .Select(s => new { s.SalesOrderID, s.OrderNumber })
                .ToList();
            ViewData["OperationsID"] = new SelectList(salesOrders, "SalesOrderID", "OrderNumber", purchaseOrder.OperationsID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.OperationsSchedule)
                .ThenInclude(os => os.SalesOrder)
                .FirstOrDefaultAsync(p => p.PurchaseOrderID == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder != null)
            {
                _context.PurchaseOrders.Remove(purchaseOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
