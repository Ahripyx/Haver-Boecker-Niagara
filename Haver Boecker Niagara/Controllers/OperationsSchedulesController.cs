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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

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
            if (ModelState.IsValid)
            {
                _context.OperationsSchedules.Add(operationsSchedule);
                await _context.SaveChangesAsync();

                if (selectedPurchaseOrders != null && selectedPurchaseOrders.Any())
                {
                    var purchaseOrders = await _context.PurchaseOrders
                        .Where(po => selectedPurchaseOrders.Contains(po.PurchaseOrderID))
                        .ToListAsync();

                    operationsSchedule.SalesOrder.PurchaseOrders = purchaseOrders;
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

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
                try
                {
                    var existingSchedule = await _context.OperationsSchedules
                        .Include(os => os.SalesOrder.PurchaseOrders)
                        .FirstOrDefaultAsync(os => os.OperationsID == id);

                    if (existingSchedule == null)
                    {
                        return NotFound();
                    }

                    existingSchedule.SalesOrderID = operationsSchedule.SalesOrderID;
                    existingSchedule.DeliveryDate = operationsSchedule.DeliveryDate;
                    existingSchedule.PreOrderNotes = operationsSchedule.PreOrderNotes;
                    existingSchedule.ScopeNotes = operationsSchedule.ScopeNotes;
                    existingSchedule.ActualAssemblyHours = operationsSchedule.ActualAssemblyHours;
                    existingSchedule.ActualReworkHours = operationsSchedule.ActualReworkHours;
                    existingSchedule.BudgetedAssemblyHours = operationsSchedule.BudgetedAssemblyHours;
                    existingSchedule.NamePlateStatus = operationsSchedule.NamePlateStatus;
                    existingSchedule.ExtraNotes = operationsSchedule.ExtraNotes;
                    if (selectedPurchaseOrders != null)
                    {
                        var purchaseOrders = await _context.PurchaseOrders
                            .Where(po => selectedPurchaseOrders.Contains(po.PurchaseOrderID))
                            .ToListAsync();

                        existingSchedule.SalesOrder.PurchaseOrders = purchaseOrders;
                    }

                    _context.Update(existingSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.OperationsSchedules.Any(e => e.OperationsID == id))
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



        public IActionResult DownloadMachineSchedule()
        {
            var operationsSchedules = _context.OperationsSchedules
                .Include(o => o.SalesOrder)
                    .ThenInclude(s => s.Customer)
                .Include(o => o.SalesOrder.Machines)
                .Include(o => o.SalesOrder.EngineeringPackage)
                    .ThenInclude(ep => ep.Engineers)
                .Include(o => o.SalesOrder.PurchaseOrders)
                    .ThenInclude(po => po.Vendor)
                .Select(o => new
                {
                    o.OperationsID,
                    SalesOrder = o.SalesOrder.OrderNumber,
                    CustomerName = o.SalesOrder.Customer.Name,
                    Machines = string.Join(", ", o.SalesOrder.Machines.Select(m => m.MachineDescription)),
                    SerialAndIPO = string.Join(", ", o.SalesOrder.Machines.Select(m => $"{m.SerialNumber} / {m.InternalPONumber}")),
                    EngineeringInfo = string.Join(" \n ", new string[]
                    {
            string.Join(", ", o.SalesOrder.EngineeringPackage.Engineers.Select(e => e.Name)),
            o.SalesOrder.EngineeringPackage.EstimatedReleaseSummary,
            o.SalesOrder.EngineeringPackage.EstimatedApprovalSummary
                    }),
                    Vendors = string.Join("\n ", o.SalesOrder.PurchaseOrders.Select(po => po.Vendor.Name)),
                    PurchaseOrders = string.Join("\n ", o.SalesOrder.PurchaseOrders.Select(po => $"{po.PurchaseOrderNumber} ({po.PODueDateSummary})")),
                    DeliveryDate = o.DeliveryDate,
                    MediaInfo = string.Join(" \n ", new string[]
                    {
            o.SalesOrder.Media ? "✓ Media" : "✗ Media",
            o.SalesOrder.SparePartsMedia ? "✓ SparePartsMedia" : "✗ SparePartsMedia",
            o.SalesOrder.Base ? "✓ Base" : "✗ Base",
            o.SalesOrder.AirSeal ? "✓ AirSeal" : "✗ AirSeal",
            o.SalesOrder.CoatingOrLining ? "✓ CoatingOrLining" : "✗ CoatingOrLining",
            o.SalesOrder.Disassembly ? "✓ Disassembly" : "✗ Disassembly"
                    }),

                    NotesInfo = string.Join(" \n ", new string[]
                    {
            o.PreOrderNotes ?? "No PreOrderNotes",
            o.ScopeNotes ?? "No ScopeNotes",
            $"Actual Assembly Hours: {o.ActualAssemblyHours}",
            $"Actual Rework Hours: {o.ActualReworkHours}",
            $"Budgeted Assembly Hours: {o.BudgetedAssemblyHours}",
            o.NamePlateStatus ? "NamePlate: Received" : "NamePlate: Required",
            o.ExtraNotes ?? "No ExtraNotes"
                    })
                });



            int numRows = operationsSchedules.Count();

            if (numRows > 0)
            {
                using (ExcelPackage excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("MachineSchedules");

                    workSheet.Cells[3, 1].LoadFromCollection(operationsSchedules, true);

                    workSheet.Column(9).Style.Numberformat.Format = "yyyy-mm-dd";

                    using (ExcelRange headings = workSheet.Cells[3, 1, 3, 11])
                    {
                        headings.Style.Font.Bold = true;
                        headings.Style.Font.Size = 14;
                        var fill = headings.Style.Fill;
                        fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.LightGray);
                        headings.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        headings.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    workSheet.Cells[4, 1, numRows + 3, 11].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    workSheet.Row(3).Height = 25;

                    for (int i = 2; i <= 11; i++)
                    {
                        workSheet.Column(i).Width = 50;
                    }

                    for (int i = 4; i <= numRows + 3; i++)
                    {
                        workSheet.Row(i).Height = 100;
                        workSheet.Cells[i, 1, i, 11].Style.Font.Size = 12;
                        workSheet.Cells[i, 1, i, 11].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    workSheet.Cells[1, 1].Value = "Machine Schedules";
                    using (ExcelRange Rng = workSheet.Cells[1, 1, 1, 11])
                    {
                        Rng.Merge = true;
                        Rng.Style.Font.Bold = true;
                        Rng.Style.Font.Size = 20;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    DateTime utcDate = DateTime.UtcNow;
                    TimeZoneInfo esTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime localDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, esTimeZone);
                    using (ExcelRange Rng = workSheet.Cells[2, 1])
                    {
                        Rng.Value = localDate.ToShortDateString();
                        Rng.Style.Font.Bold = true;
                        Rng.Style.Font.Size = 12;
                        Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    try
                    {
                        Byte[] theData = excel.GetAsByteArray();
                        string filename = "MachineSchedules.xlsx";
                        string mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        return File(theData, mimeType, filename);
                    }
                    catch (Exception)
                    {
                        return BadRequest("Could not build and download the file.");
                    }
                }
            }
            return NotFound("No data.");
        }
        private bool OperationsScheduleExists(int id)
        {
            return _context.OperationsSchedules.Any(e => e.OperationsID == id);
        }
    }
}