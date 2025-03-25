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
using Microsoft.AspNetCore.Authorization;

namespace Haver_Boecker_Niagara.Controllers
{
    [Authorize(Roles = "admin")]
    public class PurchaseOrdersController : ElephantController
    {
        private readonly HaverContext _context;

        public PurchaseOrdersController(HaverContext context)
        {
            _context = context;
        }

    // GET: PurchaseOrders
    [Authorize(Roles = "admin, procurement, pic, read only")]

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
            string[] sortOptions = { "PurchaseOrderNumber", "VendorName", "OrderNumber", "PODueDate" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            var purchaseOrders = _context.PurchaseOrders
                .Include(p => p.Vendor)
                .Include(p => p.SalesOrder)  
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

            var query = purchaseOrders.Select(p => new
            {
                p.PurchaseOrderID,
                p.PurchaseOrderNumber,
                p.POActualDueDate,
                VendorName = p.Vendor.Name,
                OrderNumber = p.SalesOrder.OrderNumber, 
                p.PODueDate
            });

            query = sortField switch
            {
                "PurchaseOrderNumber" => sortDirection == "asc"
                    ? query.OrderBy(p => p.PurchaseOrderNumber)
                    : query.OrderByDescending(p => p.PurchaseOrderNumber),
                "VendorName" => sortDirection == "asc"
                    ? query.OrderBy(p => p.VendorName)
                    : query.OrderByDescending(p => p.VendorName),
                "OrderNumber" => sortDirection == "asc"
                    ? query.OrderBy(p => p.OrderNumber)
                    : query.OrderByDescending(p => p.OrderNumber),
                "PODueDate" => sortDirection == "asc"
                    ? query.OrderBy(p => p.PODueDate)
                    : query.OrderByDescending(p => p.PODueDate),
                _ => query.OrderBy(p => p.PurchaseOrderNumber)
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<PurchaseOrder>.CreateAsync(
                query.Select(p => new PurchaseOrder
                {
                    PurchaseOrderID = p.PurchaseOrderID,
                    PurchaseOrderNumber = p.PurchaseOrderNumber,
                    PODueDate = p.PODueDate,
                    POActualDueDate = p.POActualDueDate,
                    Vendor = new Vendor { Name = p.VendorName },
                    SalesOrder = new SalesOrder { OrderNumber = p.OrderNumber },
                }),
                page ?? 1,
                pageSize
            );

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
                .FirstOrDefaultAsync(p => p.PurchaseOrderID == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }
        [Authorize(Roles = "admin, procurement, pic, read only")]

        // GET: PurchaseOrders/Create
        public IActionResult Create()
        {
            var salesOrders = _context.SalesOrders
                .Select(s => new { s.SalesOrderID, s.OrderNumber })
                .ToList();
            ViewData["SalesOrderID"] = new SelectList(salesOrders, "SalesOrderID", "OrderNumber");
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name");
            return View();
        }

        // POST: PurchaseOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, procurement, pic, read only")]

        public async Task<IActionResult> Create([Bind("PurchaseOrderID,PurchaseOrderNumber,PODueDate,VendorID,SalesOrderID")] PurchaseOrder purchaseOrder)
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
            ViewData["SalesOrderID"] = new SelectList(salesOrders, "SalesOrderID", "OrderNumber", purchaseOrder.SalesOrderID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        [Authorize(Roles = "admin, procurement, pic, read only")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(p => p.PurchaseOrderID == id);

            if (purchaseOrder == null)
            {
                return NotFound();
            }

            ViewBag.VendorID = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);

            var salesOrders = _context.SalesOrders
                .Select(s => new { s.SalesOrderID, s.OrderNumber })
                .ToList();
            ViewBag.SalesOrderID = new SelectList(salesOrders, "SalesOrderID", "OrderNumber", purchaseOrder.SalesOrderID);

            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, procurement, pic, read only")]

        public async Task<IActionResult> Edit(int id, [Bind("PurchaseOrderID,PurchaseOrderNumber,PODueDate,VendorID,SalesOrderID")] PurchaseOrder purchaseOrder)
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
            ViewData["SalesOrderID"] = new SelectList(salesOrders, "SalesOrderID", "OrderNumber", purchaseOrder.SalesOrderID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        [Authorize(Roles = "admin, procurement, pic, read only")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrders
                .Include(p => p.Vendor)
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
        [Authorize(Roles = "admin, procurement, pic, read only")]

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
