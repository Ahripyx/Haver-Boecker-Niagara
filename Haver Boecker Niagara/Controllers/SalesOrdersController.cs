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
using Haver_Boecker_Niagara.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.AccessControl;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Drawing;


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
                .Include(s => s.PurchaseOrders)
                .FirstOrDefaultAsync(m => m.SalesOrderID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        public IActionResult Create()
        {
            SalesOrder salesOrder = new SalesOrder();
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name");

            ViewData["PurchaseOrders"] = new SelectList(_context.PurchaseOrders, "PurchaseOrderID", "PurchaseOrderNumber");
            PopulatePurchaseOrders(salesOrder);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string[] selectedOptions,
            [Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly")] SalesOrder salesOrder,
            List<int> SelectedPurchaseOrderIds)

        {
            UpdatePurchaseOrders(selectedOptions, salesOrder);
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
            PopulatePurchaseOrders(salesOrder);
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

            PopulatePurchaseOrders(salesOrder);

            return View(salesOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, string[] selectedOptions, [Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,EngineeringPackageID")] SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderID)
            {
                return NotFound();
            }

            UpdatePurchaseOrders(selectedOptions, salesOrder);

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
            PopulatePurchaseOrders(salesOrder);

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

        public async Task<IActionResult> DownloadMachineSchedule()
        {
            var salesOrders = await _context.SalesOrders
                .Include(s => s.Customer)
                .Include(s => s.Machines)
                .Include(s => s.EngineeringPackage)
                    .ThenInclude(ep => ep.Engineers)
                .Include(s => s.PurchaseOrders)
                    .ThenInclude(po => po.Vendor)
                .AsNoTracking()
                .ToListAsync();

            if (!salesOrders.Any())
                return NotFound("No data available.");

            using var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("MachineSchedules");

            // Headers
            var headers = new string[]
            {
            "Sales Order", "Customer Name", "Machines", "Serial & IPO", "Engineering Info",
            "Vendors", "Purchase Orders", "Delivery Date", "Notes"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                workSheet.Cells[1, i + 1].Value = headers[i];
                workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                workSheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                workSheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            // Data Rows
            int row = 2;
            foreach (var order in salesOrders)
            {
                workSheet.Cells[row, 1].Value = order.OrderNumber;
                workSheet.Cells[row, 2].Value = order.Customer?.Name ?? "N/A";
                workSheet.Cells[row, 3].Value = string.Join(", ", order.Machines.Select(m => m.MachineDescription));
                workSheet.Cells[row, 4].Value = string.Join(", ", order.Machines.Select(m => $"{m.SerialNumber} / {m.InternalPONumber}"));
                workSheet.Cells[row, 5].Value = string.Join("\n", new string[]
                {
                string.Join(", ", order.EngineeringPackage?.Engineers?.Select(e => e.Name) ?? new List<string>()),
                order.EngineeringPackage?.EstimatedReleaseSummary ?? "N/A",
                order.EngineeringPackage?.EstimatedApprovalSummary ?? "N/A"
                });

                workSheet.Cells[row, 6].Value = string.Join("\n", order.PurchaseOrders.Select(po => po.Vendor?.Name ?? "N/A"));
                workSheet.Cells[row, 7].Value = string.Join("\n", order.PurchaseOrders.Select(po => $"{po.PurchaseOrderNumber} ({po.PODueDateSummary})"));
                workSheet.Cells[row, 8].Value = order.EstimatedCompletionSummary + " (" + order.ActualCompletionSummary + ")";
                workSheet.Cells[row, 9].Value = order.ExtraNotes ?? "No Extra Notes";

                row++;
            }

            // Auto-fit columns for better display
            workSheet.Cells.AutoFitColumns();

            var fileData = excel.GetAsByteArray();
            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MachineSchedules.xlsx");
        }

        private bool SalesOrderExists(int id)
        {
            return _context.SalesOrders.Any(e => e.SalesOrderID == id);
        }

        private void PopulatePurchaseOrders(SalesOrder salesOrder)
        {
            var allOptions = _context.PurchaseOrders;
            var currentOptionHS = new HashSet<int>(salesOrder.PurchaseOrders.Select(p => p.PurchaseOrderID));

            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var p in allOptions)
            {
                if (currentOptionHS.Contains(p.PurchaseOrderID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = p.PurchaseOrderID,
                        DisplayText = p.PurchaseOrderNumber
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = p.PurchaseOrderID,
                        DisplayText = p.PurchaseOrderNumber
                    });
                }

                ViewData["selOpts"] = new MultiSelectList(selected.OrderBy(p => p.DisplayText), "ID", "DisplayText");
                ViewData["availOpts"] = new MultiSelectList(available.OrderBy(p => p.DisplayText), "ID", "DisplayText");
            }
        }

        private void UpdatePurchaseOrders(string[] selectedOptions, SalesOrder salesOrderToUpdate)
        {
            if (selectedOptions == null)
            {
                salesOrderToUpdate.PurchaseOrders = new List<PurchaseOrder>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var currentOptionsHS = new HashSet<int>(salesOrderToUpdate.PurchaseOrders.Select(p => p.PurchaseOrderID));
            foreach (var p in _context.PurchaseOrders)
            {
                if (selectedOptionsHS.Contains(p.PurchaseOrderID.ToString()))
                {
                    if (!currentOptionsHS.Contains(p.PurchaseOrderID))
                    {
                        salesOrderToUpdate.PurchaseOrders.Add(new PurchaseOrder
                        {
                            PurchaseOrderID = p.PurchaseOrderID,
                            SalesOrderID = salesOrderToUpdate.SalesOrderID
                        });
                    }
                }
                else
                {
                    if (currentOptionsHS.Contains(p.PurchaseOrderID))
                    {
                        PurchaseOrder? purchaseOrderToRemove = salesOrderToUpdate.PurchaseOrders
                            .FirstOrDefault(p => p.PurchaseOrderID == p.PurchaseOrderID);
                        if (purchaseOrderToRemove != null)
                        {
                            _context.Remove(purchaseOrderToRemove);
                        }
                    }
                }
            }
        }
    }
}
