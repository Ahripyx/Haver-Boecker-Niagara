using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.CustomControllers;
using Haver_Boecker_Niagara.Utilities;

namespace Haver_Boecker_Niagara.Controllers
{
    public class SalesOrderController : ElephantController
    {
        private readonly HaverContext _context;

        public SalesOrderController(HaverContext context)
        {
            _context = context;
        }

        // GET: SalesOrder
        public async Task<IActionResult> Index(decimal? minPrice, decimal? maxPrice, string? searchStatus, string? searchCustomerName, int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "OrderNumber")
        {
            string[] sortOptions = { "Price", "Status", "OrderNumber", "CustomerName" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;
            var salesOrders = _context.SalesOrders
                .Include(g => g.Customer)
                .Include(g => g.EngineeringPackage)
                .AsNoTracking();
            if (minPrice.HasValue)
            {
                salesOrders = salesOrders.Where(g => g.Price >= minPrice.Value);
                filterCount++;
            }
            if (maxPrice.HasValue)
            {
                salesOrders = salesOrders.Where(g => g.Price <= maxPrice.Value);
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchStatus))
            {
                salesOrders = salesOrders.Where(g => EF.Functions.Like(g.Status, $"{searchStatus}"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchCustomerName))
            {
                salesOrders = salesOrders.Where(g => EF.Functions.Like(g.Customer.Name, $"%{searchCustomerName}%"));
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

            salesOrders = sortField switch
            {
                "OrderNumber" => sortDirection == "asc" ? salesOrders.OrderBy(g => g.OrderNumber) : salesOrders.OrderByDescending(g => g.OrderNumber),
                "CustomerName" => sortDirection == "asc" ? salesOrders.OrderBy(g => g.Customer.Name) : salesOrders.OrderByDescending(g => g.Customer.Name),
                "Status" => sortDirection == "asc" ? salesOrders.OrderBy(g => g.Status) : salesOrders.OrderByDescending(g => g.Status),
                _ => sortDirection == "asc" ? salesOrders.OrderBy(g => g.OrderNumber) : salesOrders.OrderByDescending(g => g.OrderNumber)
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<SalesOrder>.CreateAsync(salesOrders, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: SalesOrder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.EngineeringPackage)
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // GET: SalesOrder/Create
        public IActionResult Create()
        { 
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name");
            ViewData["EngineeringPackageID"] = new SelectList(_context.EngineeringPackages, "EngineeringPackageID", "EngineeringPackageID");
            return View();
        }

        // POST: SalesOrder/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,EngineeringPackageID")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", salesOrder.CustomerID);
            ViewData["EngineeringPackageID"] = new SelectList(_context.EngineeringPackages, "EngineeringPackageID", "EngineeringPackageID", salesOrder.EngineeringPackageID);
            return View(salesOrder);
        }

        // GET: SalesOrder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", salesOrder.CustomerID);
            ViewData["EngineeringPackageID"] = new SelectList(_context.EngineeringPackages, "EngineeringPackageID", "EngineeringPackageID", salesOrder.EngineeringPackageID);
            return View(salesOrder);
        }

        // POST: SalesOrder/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,EngineeringPackageID")] SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.SalesOrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", salesOrder.CustomerID);
            ViewData["EngineeringPackageID"] = new SelectList(_context.EngineeringPackages, "EngineeringPackageID", "EngineeringPackageID", salesOrder.EngineeringPackageID);
            return View(salesOrder);
        }

        // GET: SalesOrder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.EngineeringPackage)
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders.FindAsync(id);
            if (salesOrder != null)
            {
                _context.SalesOrders.Remove(salesOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.SalesOrderID == id);
        }
    }
}
