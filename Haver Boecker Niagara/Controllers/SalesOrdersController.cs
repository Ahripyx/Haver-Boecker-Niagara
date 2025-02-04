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
using System.Reflection.PortableExecutable;

namespace Haver_Boecker_Niagara.Controllers
{
    public class SalesOrdersController : ElephantController
    {
        private readonly HaverContext _context;

        public SalesOrdersController(HaverContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(
            string? searchOrderNo,
            string? searchCustomer,
            string? searchStatus,
            int? page,
            int? pageSizeID,
            string? actionButton,
            string sortDirection = "asc",
            string sortField = "OrderNumber"
        )
        {
            string[] sortOptions = { "OrderNumber", "CustomerName", "Status", "Price" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;
            var salesOrders = _context.SalesOrders
                .Include(p => p.Machines)
                .Include(p => p.Customer)
                .Include(p => p.PurchaseOrders)
                .ThenInclude(po => po.Vendor)
                .Include(p => p.EngineeringPackage)
                .ThenInclude(po => po.Engineers)
                .AsNoTracking();


            ViewBag.SalesOrders = new SelectList(await _context.SalesOrders.Select(s => new { s.OrderNumber, s.SalesOrderID }).ToListAsync(), "OrderNumber", "OrderNumber");
            ViewBag.Customers = new SelectList(await _context.Customers.Select(c => new { c.CustomerID, c.Name }).ToListAsync(), "Name", "Name");

            if (!string.IsNullOrEmpty(searchOrderNo))
            {
                salesOrders = salesOrders.Where(s => s.OrderNumber.Contains(searchOrderNo));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchCustomer))
            {
                salesOrders = salesOrders.Where(s => s.Customer.Name.Contains(searchCustomer));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchStatus))
            {
                salesOrders = salesOrders.Where(s => s.Status.Contains(searchStatus));
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
                "OrderNumber" => sortDirection == "asc"
                    ? salesOrders.OrderBy(s => s.OrderNumber)
                    : salesOrders.OrderByDescending(s => s.OrderNumber),
                "CustomerName" => sortDirection == "asc"
                    ? salesOrders.OrderBy(s => s.Customer.Name)
                    : salesOrders.OrderByDescending(s => s.Customer.Name),
                "Status" => sortDirection == "asc"
                    ? salesOrders.OrderBy(s => s.Status)
                    : salesOrders.OrderByDescending(s => s.Status),
                "Price" => sortDirection == "asc"
                    ? salesOrders.OrderBy(s => s.Price)
                    : salesOrders.OrderByDescending(s => s.Price),
                _ => salesOrders.OrderBy(s => s.OrderNumber)
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<SalesOrder>.CreateAsync(salesOrders, page ?? 1, pageSize);

            return View(pagedData);
        }



        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.EngineeringPackage)
                .Include(s => s.Machines)
                .Include(s =>s.PurchaseOrders)
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID");
            ViewData["PurchaseOrders"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderNumber");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,CompletionDate,ActualCompletionDate,ExtraNotes,EngineeringPackageID")] SalesOrder salesOrder , List<int> SelectedPurchaseOrderIds)
        {
            if (ModelState.IsValid)
            {
                DateTime today = DateTime.Today;
                int daysUntilNextFriday = ((int)DayOfWeek.Friday - (int)today.DayOfWeek + 7) % 7;
                DateTime nextFriday = today.AddDays(daysUntilNextFriday);

                var newEngineeringPackage = new EngineeringPackage
                {
                    PackageReleaseDate = nextFriday,
                    ApprovalDrawingDate = nextFriday,
                    Engineers = new HashSet<Engineer>()
                };

                _context.EngineeringPackages.Add(newEngineeringPackage);
                await _context.SaveChangesAsync();

                salesOrder.EngineeringPackageID = newEngineeringPackage.EngineeringPackageID;

                _context.SalesOrders.Add(salesOrder);
                await _context.SaveChangesAsync();

                if (SelectedPurchaseOrderIds != null && SelectedPurchaseOrderIds.Any())
                {
                    foreach (var purchaseOrderId in SelectedPurchaseOrderIds)
                    {
                        var purchaseOrder = await _context.PurchaseOrders.FindAsync(purchaseOrderId);
                        if (purchaseOrder != null)
                        {
                            salesOrder.PurchaseOrders.Add(purchaseOrder);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", salesOrder.CustomerID);
            ViewData["PurchaseOrders"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderNumber");
            return View(salesOrder);
        }
        // GET: SalesOrders/Edit/5
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
            ViewData["PurchaseOrders"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderNumber");
            return View(salesOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalesOrderID,Price,Status,CustomerID,CompletionDate,ActualCompletionDate,ExtraNotes")] SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingOrder = await _context.SalesOrders
                        .FirstOrDefaultAsync(s => s.SalesOrderID == id);

                    if (existingOrder == null)
                    {
                        return NotFound();
                    }

                    salesOrder.EngineeringPackageID = existingOrder.EngineeringPackageID;
                    salesOrder.OrderNumber = existingOrder.OrderNumber;

                    existingOrder.Price = salesOrder.Price;
                    existingOrder.Status = salesOrder.Status;
                    existingOrder.CustomerID = salesOrder.CustomerID;
                    existingOrder.CompletionDate = salesOrder.CompletionDate;
                    existingOrder.ActualCompletionDate = salesOrder.ActualCompletionDate;
                    existingOrder.ExtraNotes = salesOrder.ExtraNotes;

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
            ViewData["PurchaseOrders"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderNumber");
            return View(salesOrder);
        }


        // GET: SalesOrders/Delete/5
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

        // POST: SalesOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salesOrder = await _context.SalesOrders
                .Include(s => s.EngineeringPackage)
                .FirstOrDefaultAsync(s => s.SalesOrderID == id);

            if (salesOrder != null)
            {
                if (salesOrder.EngineeringPackage != null)
                {
                    int packageId = salesOrder.EngineeringPackage.EngineeringPackageID;

                    var relatedSpecialities = _context.EngineeringPackageEngineers
                        .Where(es => es.EngineeringPackageID == packageId);
                    _context.EngineeringPackageEngineers.RemoveRange(relatedSpecialities);
                    _context.EngineeringPackages.Remove(salesOrder.EngineeringPackage);
                }
                _context.SalesOrders.Remove(salesOrder);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.SalesOrderID == id);
        }
    }
}
