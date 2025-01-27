﻿using System;
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
    public class SalesOrdersController : ElephantController
    {
        private readonly HaverContext _context;

        public SalesOrdersController(HaverContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            int? pageSizeID,
            int? page,
            string? SearchOrderNo,
            string? SearchCustomer,
            string? SearchStatus,
            string? actionButton,
            string sortDirection = "asc",
            string sortField = ""
        )
        {
            string[] sortOptions = { "OrderNumber", "Customer", "Status", "Price" };
            var filterCount = 0;
            ViewData["Filtering"] = "btn-outline-secondary";

            IQueryable<SalesOrder> salesOrders = _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.EngineeringPackage)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(SearchOrderNo))
            {
                salesOrders = salesOrders.Where(s => s.OrderNumber.ToLower().Contains(SearchOrderNo.ToLower()));
                filterCount++;
            }

            if (!string.IsNullOrEmpty(SearchCustomer))
            {
                salesOrders = salesOrders.Where(s => s.Customer.Name.ToLower().Contains(SearchCustomer.ToLower()));
                filterCount++;
            }

            if (!string.IsNullOrEmpty(SearchStatus))
            {
                salesOrders = salesOrders.Where(s => s.Status.ToLower().Contains(SearchStatus.ToLower()));
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

                "Customer" => sortDirection == "asc"
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
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name");
            ViewData["PurchaseOrders"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly")] SalesOrder salesOrder,
            List<int> SelectedPurchaseOrderIds)
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

        // POST: SalesOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,EngineeringPackageID")] SalesOrder salesOrder)
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
