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
using Haver_Boecker_Niagara.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Haver_Boecker_Niagara.Utilities;
using Elfie.Serialization;

namespace Haver_Boecker_Niagara.Controllers
{
    public class OperationsSchedulesController : ElephantController
    {
        private readonly HaverContext _context;

        public OperationsSchedulesController(HaverContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchSales, string? searchCustomer, string? searchDelivery,
                                               int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "searchSales")
        {
            string[] sortOptions = { "SalesOrder", "CustomerName", "DeliveryDate" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            var operationsSchedules = _context.OperationsSchedules
                .Include(o => o.SalesOrder)
                    .ThenInclude(s => s.Customer)
                .Include(o => o.SalesOrder.Machines)
                .Include(o => o.SalesOrder.EngineeringPackage)
                    .ThenInclude(ep => ep.Engineers)
                 .Include(o => o.SalesOrder.PurchaseOrders)
                    .ThenInclude(po => po.Vendor)
                .Select(o => new OperationsScheduleViewModel
                {
                    Id = o.OperationsID,
                    SalesOrder = o.SalesOrder.OrderNumber,
                    CustomerName = o.SalesOrder.Customer.Name,
                    Machines = o.SalesOrder.Machines
                        .Select(m => m.MachineDescription)
                        .ToList(),
                    SerialAndIPO = o.SalesOrder.Machines
                        .Select(m => $"{m.SerialNumber} / {m.InternalPONumber}")
                        .ToList(),
                    PackageRelease = new PackageReleaseViewModel
                    {
                        Engineers = o.SalesOrder.EngineeringPackage != null && o.SalesOrder.EngineeringPackage.Engineers != null
                    ? o.SalesOrder.EngineeringPackage.Engineers.Select(e => e.Name).ToList()
                    : new List<string>(),
                        EstimatedReleaseSummary = o.SalesOrder.EngineeringPackage != null && o.SalesOrder.EngineeringPackage.EstimatedReleaseSummary != null
                    ? o.SalesOrder.EngineeringPackage.EstimatedReleaseSummary
                    : "N/A",
                        EstimatedApprovalSummary = o.SalesOrder.EngineeringPackage != null && o.SalesOrder.EngineeringPackage.EstimatedApprovalSummary != null
                    ? o.SalesOrder.EngineeringPackage.EstimatedApprovalSummary
                    : "N/A"
                    },
                    Vendors = o.SalesOrder.PurchaseOrders
                        .Select(po => po.Vendor.Name)
                        .ToList(),
                    PurchaseOrders = o.SalesOrder.PurchaseOrders
                        .Select(po => $"{po.PurchaseOrderNumber} ({po.PODueDateSummary})")
                        .ToList(),
                    DeliveryDate = o.DeliveryDate,
                    AdditionalDetails = new AdditionalDetailsViewModel
                    {
                        Media = o.SalesOrder.Media ? "✓" : "✗",
                        SparePartsMedia = o.SalesOrder.SparePartsMedia ? "✓" : "✗",
                        Base = o.SalesOrder.Base ? "✓" : "✗",
                        AirSeal = o.SalesOrder.AirSeal ? "✓" : "✗",
                        CoatingOrLining = o.SalesOrder.CoatingOrLining ? "✓" : "✗",
                        Disassembly = o.SalesOrder.Disassembly ? "✓" : "✗"
                    },
                    Notes = new NotesViewModel
                    {
                        PreOrderNotes = o.PreOrderNotes,
                        ScopeNotes = o.ScopeNotes,
                        ActualAssemblyHours = o.ActualAssemblyHours,
                        ActualReworkHours = o.ActualReworkHours,
                        BudgetedAssemblyHours = o.BudgetedAssemblyHours,
                        NamePlateStatus = o.NamePlateStatus ? "Received" : "Required",
                        ExtraNotes = o.ExtraNotes
                    }
                });


            ViewBag.SalesOrders = new SelectList(await _context.SalesOrders.Select(s => new { s.OrderNumber, s.SalesOrderID }).ToListAsync(), "OrderNumber", "OrderNumber");
            ViewBag.Customers = new SelectList(await _context.Customers.Select(c => new { c.CustomerID, c.Name }).ToListAsync(), "Name", "Name");

            if (!string.IsNullOrEmpty(searchSales))
            {
                operationsSchedules = operationsSchedules.Where(o => o.SalesOrder.Contains(searchSales));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchCustomer))
            {
                operationsSchedules = operationsSchedules.Where(o => o.CustomerName.Contains(searchCustomer));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchDelivery) && DateTime.TryParse(searchDelivery, out DateTime searchAfterDate))
            {
                operationsSchedules = operationsSchedules.Where(o => o.DeliveryDate >= searchAfterDate);
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

            operationsSchedules = sortField switch
            {
                "SalesOrder" => sortDirection == "asc" ? operationsSchedules.OrderBy(o => o.SalesOrder) : operationsSchedules.OrderByDescending(o => o.SalesOrder),
                "CustomerName" => sortDirection == "asc" ? operationsSchedules.OrderBy(o => o.CustomerName) : operationsSchedules.OrderByDescending(o => o.CustomerName),
                "DeliveryDate" => sortDirection == "asc" ? operationsSchedules.OrderBy(o => o.DeliveryDate) : operationsSchedules.OrderByDescending(o => o.DeliveryDate),
                _ => operationsSchedules.OrderBy(o => o.SalesOrder),
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);

            var pagedData = await PaginatedList<OperationsScheduleViewModel>.CreateAsync(operationsSchedules, page ?? 1, pageSize);

            return View(pagedData);
        }


        // GET: OperationsSchedules/Details/5
        public async Task<IActionResult> Details(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return NotFound();
            }
            var operationsSchedule = await _context.OperationsSchedules
                .Include(o => o.SalesOrder)
                    .ThenInclude(s => s.Customer)
                .Include(o => o.SalesOrder.Machines)
                .Include(o => o.SalesOrder.EngineeringPackage)
                    .ThenInclude(ep => ep.Engineers)
                 .Include(o => o.SalesOrder.PurchaseOrders)
                    .ThenInclude(po => po.Vendor)
                .FirstOrDefaultAsync(m => m.SalesOrder.OrderNumber == orderNumber);

            if (operationsSchedule == null)
            {
                return NotFound();
            }

            var viewModel = new OperationsScheduleViewModel
            {
                Id = operationsSchedule.OperationsID,
                SalesOrder = operationsSchedule.SalesOrder.OrderNumber,
                CustomerName = operationsSchedule.SalesOrder.Customer.Name,
                Machines = operationsSchedule.SalesOrder.Machines
                    .Select(m => m.MachineDescription)
                    .ToList(),
                SerialAndIPO = operationsSchedule.SalesOrder.Machines
                    .Select(m => $"{m.SerialNumber} / {m.InternalPONumber}")
                    .ToList(),
                PackageRelease = new PackageReleaseViewModel
                {
                    Engineers = operationsSchedule.SalesOrder.EngineeringPackage.Engineers
                        .Select(e => e.Name)
                        .ToList(),
                    EstimatedReleaseSummary = operationsSchedule.SalesOrder.EngineeringPackage.EstimatedReleaseSummary,
                    EstimatedApprovalSummary = operationsSchedule.SalesOrder.EngineeringPackage.EstimatedApprovalSummary
                },
                Vendors = operationsSchedule.SalesOrder.PurchaseOrders
                    .Select(po => po.Vendor.Name)
                    .ToList(),
                PurchaseOrders = operationsSchedule.SalesOrder.PurchaseOrders
                    .Select(po => $"{po.PurchaseOrderNumber} ({po.PODueDateSummary})")
                    .ToList(),
                DeliveryDate = operationsSchedule.DeliveryDate,
                AdditionalDetails = new AdditionalDetailsViewModel
                {
                    Media = operationsSchedule.SalesOrder.Media ? "✓" : "✗",
                    SparePartsMedia = operationsSchedule.SalesOrder.SparePartsMedia ? "✓" : "✗",
                    Base = operationsSchedule.SalesOrder.Base ? "✓" : "✗",
                    AirSeal = operationsSchedule.SalesOrder.AirSeal ? "✓" : "✗",
                    CoatingOrLining = operationsSchedule.SalesOrder.CoatingOrLining ? "✓" : "✗",
                    Disassembly = operationsSchedule.SalesOrder.Disassembly ? "✓" : "✗"
                },
                Notes = new NotesViewModel
                {
                    PreOrderNotes = operationsSchedule.PreOrderNotes,
                    ScopeNotes = operationsSchedule.ScopeNotes,
                    ActualAssemblyHours = operationsSchedule.ActualAssemblyHours?.ToString() ?? "N/A",
                    ActualReworkHours = operationsSchedule.ActualReworkHours?.ToString() ?? "N/A",
                    BudgetedAssemblyHours = operationsSchedule.BudgetedAssemblyHours?.ToString() ?? "N/A",
                    NamePlateStatus = operationsSchedule.NamePlateStatus ? "Received" : "Required",
                    ExtraNotes = operationsSchedule.ExtraNotes
                }
            };

            return View(viewModel);
        }

        // GET: OperationsSchedules/Create
        public async Task<IActionResult> Create()
        {
            ViewData["SalesOrderID"] = new SelectList(await _context.SalesOrders.ToListAsync(), "SalesOrderID", "OrderNumber");
            ViewData["PurchaseOrders"] = new MultiSelectList(await _context.PurchaseOrders.ToListAsync(), "PurchaseOrderID", "PurchaseOrderNumber");

            return View();
        }

        // POST: OperationsSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationsID,SalesOrderID,DeliveryDate,PreOrderNotes,ScopeNotes,ActualAssemblyHours,ActualReworkHours,BudgetedAssemblyHours,NamePlateStatus,ExtraNotes")] OperationsSchedule operationsSchedule, List<int> selectedPurchaseOrders)
        {
            
            ViewData["SalesOrderID"] = new SelectList(await _context.SalesOrders.ToListAsync(), "SalesOrderID", "OrderNumber", operationsSchedule.SalesOrderID);
            return View(operationsSchedule);
        }

        // GET: OperationsSchedules/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationsSchedule = await _context.OperationsSchedules
                .Include(os => os.SalesOrder.PurchaseOrders)
                .FirstOrDefaultAsync(m => m.OperationsID == id);

            if (operationsSchedule == null)
            {
                return NotFound();
            }

            ViewData["SalesOrderID"] = new SelectList(await _context.SalesOrders.ToListAsync(), "SalesOrderID", "OrderNumber", operationsSchedule.SalesOrderID);

            return View(operationsSchedule);
        }

        // POST: OperationsSchedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OperationsID,SalesOrderID,DeliveryDate,PreOrderNotes,ScopeNotes,ActualAssemblyHours,ActualReworkHours,BudgetedAssemblyHours,NamePlateStatus,ExtraNotes")] OperationsSchedule operationsSchedule, List<int> selectedPurchaseOrders)
        {
            if (id != operationsSchedule.OperationsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               

                return RedirectToAction(nameof(Index));
            }

            ViewData["SalesOrderID"] = new SelectList(await _context.SalesOrders.ToListAsync(), "SalesOrderID", "OrderNumber", operationsSchedule.SalesOrderID);
            return View(operationsSchedule);
        }

        // GET: OperationsSchedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationsSchedule = await _context.OperationsSchedules
                .Include(o => o.SalesOrder)
                .FirstOrDefaultAsync(m => m.OperationsID == id);
            if (operationsSchedule == null)
            {
                return NotFound();
            }

            return View(operationsSchedule);
        }

        // POST: OperationsSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var operationsSchedule = await _context.OperationsSchedules.FindAsync(id);
            if (operationsSchedule != null)
            {
                _context.OperationsSchedules.Remove(operationsSchedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationsScheduleExists(int id)
        {
            return _context.OperationsSchedules.Any(e => e.OperationsID == id);
        }
    }
}
