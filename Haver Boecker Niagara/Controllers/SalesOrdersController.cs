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
using SQLitePCL;


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
            Status? searchStatus,
            int? page,
            int? pageSizeID,
            string? actionButton,
            string sortDirection = "asc",
            string sortField = "OrderNumber"
        )
        {
            string[] sortOptions = { "OrderNumber", "CustomerName" };
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
                salesOrders = salesOrders.Where(s => s.OrderNumber.ToLower().Contains(searchOrderNo.ToLower()));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchCustomer))
            {
                salesOrders = salesOrders.Where(s => s.Customer.Name.ToLower().Contains(searchCustomer.ToLower()));
                filterCount++;
            }
            if (searchStatus != null)
            {
                salesOrders = salesOrders.Where(s => s.Status == searchStatus);
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
                _ => salesOrders
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;
            var opts = new[] { Status.Open, Status.Closed };
            ViewData["Statuses"] = new SelectList(opts, searchStatus);
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
                .Include(s => s.PurchaseOrders!).ThenInclude(p => p.Vendor)
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
        public async Task<IActionResult> Create(
            [Bind("Price,Status,CustomerID,OrderNumber,CompletionDate,ActualCompletionDate,ExtraNotes")] SalesOrder salesOrder, string? chkAddPO = "off", string? chkAddMachine = "off")

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

                if (chkAddPO == "on" || chkAddMachine == "on")
                {
                    return RedirectToAction(nameof(Edit), new { ID = salesOrder.SalesOrderID });
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", salesOrder.CustomerID);
            return View(salesOrder);
        }
        // GET: SalesOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrders.Include(p => p.PurchaseOrders).Include(p => p.Machines).FirstOrDefaultAsync(p => p.SalesOrderID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "Name", salesOrder.CustomerID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name");
            PopulatePurchaseOrders(salesOrder);
            PopulateMachines(salesOrder);
            return View(salesOrder);
        }

        public async Task<IActionResult> CreatePurchaseOrder([Bind("PurchaseOrderNumber,PODueDate,VendorID")] PurchaseOrder purchaseOrder,
            [Bind("SalesOrderID")] SalesOrderVM salesOrderVM)
        {
            if (ModelState.IsValid)
            {
                purchaseOrder.SalesOrderID = salesOrderVM.SalesOrderID;
                _context.PurchaseOrders.Add(purchaseOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Edit), new { ID = salesOrderVM.SalesOrderID });
        }


        public async Task<IActionResult> EditPurchaseOrder([Bind("PurchaseOrderID,PurchaseOrderNumber,PODueDate,POActualDueDate,VendorID")] PurchaseOrder purchaseOrderToUpdate,
            [Bind("SalesOrderID")] SalesOrderVM salesOrderVM)
        {
            if (ModelState.IsValid)
            {
                var salesOrder = await _context.SalesOrders.Include(p => p.PurchaseOrders).Where(p=> p.SalesOrderID == salesOrderVM.SalesOrderID).FirstOrDefaultAsync();
                var purchaseOrder = await _context.PurchaseOrders.FindAsync(purchaseOrderToUpdate.PurchaseOrderID);
                if (salesOrder == null || purchaseOrder == null)
                {
                    return BadRequest();
                }
                // remove old
                _context.PurchaseOrders.Remove(purchaseOrder);
                salesOrder.PurchaseOrders.Remove(purchaseOrder);
                // add new
                _context.PurchaseOrders.Add(purchaseOrderToUpdate);
                salesOrder.PurchaseOrders.Add(purchaseOrderToUpdate);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Edit), new { ID = salesOrderVM.SalesOrderID });
        }

        public ActionResult SetPOViewBag(string selectedOptions)
        {
            if (String.IsNullOrEmpty(selectedOptions))
            {
                return BadRequest();
            }
            string[] selectedOptions2 = selectedOptions.Split(',');
            if (selectedOptions2.Count() == 1)
            {
                var purchaseOrder = _context.PurchaseOrders.Include(p => p.Vendor).Where(p => p.PurchaseOrderID == int.Parse(selectedOptions2[0])).FirstOrDefault();
                ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "Name", purchaseOrder.VendorID);
                return PartialView("_purchaseOrderModalEdit", purchaseOrder);

            } else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> EditMachine(int id, [Bind("MachineID,SerialNumber,NamePlateStatus,InternalPONumber,MachineSize,MachineClass,MachineSizeDesc,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,PreOrderNotes,ScopeNotes,BudgetedAssemblyHours,ActualAssemblyHours,ActualReworkHours")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                Machine? oldMachine = await _context.Machines
                    .Include(p => p.SalesOrders)
                    .Where(p => p.MachineID == machine.MachineID)
                    .FirstOrDefaultAsync();

                if (oldMachine == null)
                {
                    return NotFound();
                }
                SalesOrder? salesOrder = await _context.SalesOrders.FindAsync(id);
                if (salesOrder == null)
                {
                    return NotFound();
                }

                oldMachine.SerialNumber = machine.SerialNumber;
                oldMachine.NamePlateStatus = machine.NamePlateStatus;
                oldMachine.InternalPONumber = machine.InternalPONumber;
                oldMachine.MachineSize = machine.MachineSize;
                oldMachine.MachineClass = machine.MachineClass;
                oldMachine.MachineSizeDesc = machine.MachineSizeDesc;
                oldMachine.Media = machine.Media;
                oldMachine.SparePartsMedia = machine.SparePartsMedia;
                oldMachine.Base = machine.Base;
                oldMachine.AirSeal = machine.AirSeal;
                oldMachine.CoatingOrLining = machine.CoatingOrLining;
                oldMachine.Disassembly = machine.Disassembly;
                oldMachine.PreOrderNotes = machine.PreOrderNotes;
                oldMachine.ScopeNotes = machine.ScopeNotes;
                oldMachine.BudgetedAssemblyHours = machine.BudgetedAssemblyHours;
                oldMachine.ActualAssemblyHours = machine.ActualAssemblyHours;
                oldMachine.ActualReworkHours = machine.ActualReworkHours;

                _context.Machines.Update(oldMachine);

                if (!oldMachine.SalesOrders.Contains(salesOrder))
                {
                    _context.MachineSalesOrders.Add(new MachineSalesOrder { MachineID = oldMachine.MachineID, SalesOrderID = id });
                }
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Edit), new { ID = id });
        }

        public ActionResult SetMachineViewBag(string selectedOptions, string id)
        {
            if (String.IsNullOrEmpty(selectedOptions))
            {
                return BadRequest();
            }
            string[] selectedOptions2 = selectedOptions.Split(',');
            if (selectedOptions2.Count() == 1)
            {
                Machine? machine = _context.Machines.Where(p => p.MachineID == int.Parse(selectedOptions2[0])).FirstOrDefault();
                ViewData["id"] = id;
                return PartialView("_machineModalEdit", machine);

            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> CreateMachine([Bind("SalesOrderID,NamePlateStatus,SerialNumber,InternalPONumber,MachineSize,MachineClass,MachineSizeDesc,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,PreOrderNotes,ScopeNotes,BudgetedAssemblyHours,ActualAssemblyHours,ActualReworkHours")] MachineVM machine)
        {
            if (ModelState.IsValid)
            {
                var realMachine = new Machine
                {
                    SerialNumber = machine.SerialNumber,
                    NamePlateStatus = machine.NamePlateStatus,
                    InternalPONumber = machine.InternalPONumber,
                    MachineSize = machine.MachineSize,
                    MachineClass = machine.MachineClass,
                    MachineSizeDesc = machine.MachineSizeDesc,
                    Media = machine.Media,
                    SparePartsMedia = machine.SparePartsMedia,
                    Base = machine.Base,
                    AirSeal = machine.AirSeal,
                    CoatingOrLining = machine.CoatingOrLining,
                    Disassembly = machine.Disassembly,
                    PreOrderNotes = machine.PreOrderNotes,
                    ScopeNotes = machine.ScopeNotes,
                    BudgetedAssemblyHours = machine.BudgetedAssemblyHours,
                    ActualAssemblyHours = machine.ActualAssemblyHours,
                    ActualReworkHours = machine.ActualReworkHours
                };
                _context.Machines.Add(realMachine);
                await _context.SaveChangesAsync();

                _context.MachineSalesOrders.Add(new MachineSalesOrder { MachineID = realMachine.MachineID, SalesOrderID = machine.SalesOrderID });
                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Edit), new { ID = machine.SalesOrderID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedPOOptions, string[] selectedMachineOptions, [Bind("SalesOrderID,Price,Status,CustomerID,OrderNumber,CompletionDate,ActualCompletionDate,ExtraNotes,EngineeringPackageID")] SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderID)
            {
                return NotFound();
            }

            UpdatePurchaseOrders(selectedPOOptions, salesOrder);
            UpdateMachines(selectedMachineOptions, salesOrder);

            if (ModelState.IsValid)
            {
                try
                {
                    var existingOrder = await _context.SalesOrders
                        .Include(p => p.EngineeringPackage)
                        .Include(p => p.Machines)
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
            PopulateMachines(salesOrder);
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

        private void PopulateMachines(SalesOrder salesOrder)
        {
            var allOptions = _context.Machines;
            var currentOptionHS = new HashSet<int>(salesOrder.Machines.Select(p => p.MachineID));

            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();
            foreach (var p in allOptions)
            {
                if (currentOptionHS.Contains(p.MachineID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = p.MachineID,
                        DisplayText = p.SerialNumber
                    });
                } else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = p.MachineID,
                        DisplayText = p.SerialNumber
                    });
                }

                
            }
            ViewData["selMachineOpts"] = new SelectList(selected.OrderBy(p => p.DisplayText), "ID", "DisplayText");
            ViewData["availMachineOpts"] = new SelectList(available.OrderBy(p => p.DisplayText), "ID", "DisplayText");
        }

        private void PopulatePurchaseOrders(SalesOrder salesOrder)
        {
            var allOptions = _context.PurchaseOrders;
            var currentOptionHS = new HashSet<int>(salesOrder.PurchaseOrders.Select(p => p.PurchaseOrderID));

            var selected = new List<ListOptionVM>();
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

                ViewData["selPOOpts"] = new SelectList(selected.OrderBy(p => p.DisplayText), "ID", "DisplayText");
            }
        }

        private async void UpdatePurchaseOrders(string[] selectedOptions, SalesOrder salesOrderToUpdate)
        {
            if (selectedOptions == null)
            {
                salesOrderToUpdate.PurchaseOrders = new List<PurchaseOrder>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var currentOptionsHS = new HashSet<int>(
                await _context.PurchaseOrders
                .Where(p => p.SalesOrderID == salesOrderToUpdate.SalesOrderID)
                .Select(p => p.PurchaseOrderID)
                .ToListAsync()
            );
            foreach (int p in currentOptionsHS)
            {
                
                if (!selectedOptionsHS.Contains(p.ToString()))
                {
                    PurchaseOrder? purchaseOrderToRemove = await _context.PurchaseOrders.FindAsync(p);
                    if (purchaseOrderToRemove != null)
                    {
                        _context.Remove(purchaseOrderToRemove);
                    }
                }
                
            }
        }

        private async void UpdateMachines(string[] selectedOptions, SalesOrder salesOrderToUpdate)
        {
            if (selectedOptions == null)
            {
                salesOrderToUpdate.Machines = new List<Machine>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var currentOptionsHS = new HashSet<int>(
                await _context.MachineSalesOrders
                .Where(p => p.SalesOrderID == salesOrderToUpdate.SalesOrderID)
                .Select(p => p.MachineID)
                .ToListAsync()
                );
            foreach (var p in _context.Machines)
            {
                if (selectedOptionsHS.Contains(p.MachineID.ToString()))
                {
                    if (!currentOptionsHS.Contains(p.MachineID))
                    {
                        _context.MachineSalesOrders.Add(new MachineSalesOrder { MachineID = p.MachineID, SalesOrderID = salesOrderToUpdate.SalesOrderID });
                    }
                }
                else
                {
                    if (currentOptionsHS.Contains(p.MachineID))
                    {
                        MachineSalesOrder? machineToRemove = await _context.MachineSalesOrders
                            .Where(e => e.MachineID == p.MachineID && salesOrderToUpdate.SalesOrderID == e.SalesOrderID)
                            .FirstOrDefaultAsync();
                        if (machineToRemove != null)
                        {
                            _context.MachineSalesOrders.Remove(machineToRemove);
                        }
                    }
                }
            }
        }
    }
}
