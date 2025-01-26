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

namespace Haver_Boecker_Niagara.Controllers
{
    public class OperationsSchedulesController : ElephantController
    {
        private readonly HaverContext _context;

        public OperationsSchedulesController(HaverContext context)
        {
            _context = context;
        }

        // GET: OperationsSchedules
        public async Task<IActionResult> Index()
        {
            var operationsSchedules = await _context.OperationsSchedules
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
                        Engineers = o.SalesOrder.EngineeringPackage.Engineers
                            .Select(e => e.Name)
                            .ToList(),
                        EstimatedReleaseSummary = o.SalesOrder.EngineeringPackage.EstimatedReleaseSummary,
                        EstimatedApprovalSummary = o.SalesOrder.EngineeringPackage.EstimatedApprovalSummary
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
                })
                .ToListAsync();

            return View(operationsSchedules);
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
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationsSchedule = await _context.OperationsSchedules
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
            var operationsSchedule = await _context.OperationsSchedules
                .FirstOrDefaultAsync(m => m.OperationsID == id);

            if (operationsSchedule != null)
            {
                _context.OperationsSchedules.Remove(operationsSchedule);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OperationsScheduleExists(int id)
        {
            return _context.OperationsSchedules.Any(e => e.OperationsID == id);
        }
    }
}
