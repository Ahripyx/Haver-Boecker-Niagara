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
    public class MachineController : ElephantController
    {
        private readonly HaverContext _context;

        public MachineController(HaverContext context)
        {
            _context = context;
        }

        // GET: Machine
        public async Task<IActionResult> Index
        (
            int? pageSizeID, 
            int? page, 
            string? SearchSerialNo, 
            string? SearchPONo, 
            string? actionButton, 
            string sortDirection = "asc", 
            string sortField = ""
        ) {

            string[] sortOptions = { "Size", "Class", "Description" };
            
            var filterCount = 0;
            ViewData["Filtering"] = "btn-outline-secondary";

            IQueryable<Machine> machines = _context
                                           .Machines
                                           .Include(m => m.SalesOrders)
                                           .AsNoTracking();


            if (!String.IsNullOrEmpty(SearchSerialNo))
            {
                machines = machines.Where(p => p.SerialNumber
                                                .ToLower()
                                                .Contains(SearchSerialNo.ToLower()));
                filterCount++;
            }
            if (!String.IsNullOrEmpty(SearchPONo))
            {
                machines = machines.Where(p => p.InternalPONumber
                                                .ToLower()
                                                .Contains(SearchPONo.ToLower()));
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

            machines = sortField switch
            {

                
                "Size" => sortDirection == "asc" 
                ? machines.OrderBy(p => p.MachineSize) 
                : machines.OrderByDescending(p => p.MachineSize),
                
                "Class" => sortDirection == "asc" 
                ? machines.OrderBy(p => p.MachineClass) 
                : machines.OrderByDescending(p => p.MachineClass),
                
                "Description" => sortDirection == "asc" 
                ? machines.OrderBy(p => p.MachineSizeDesc) 
                : machines.OrderByDescending(p => p.MachineSizeDesc),
                _ => sortDirection == "asc" ? machines.OrderBy(p => p.MachineSize) : machines.OrderByDescending(p => p.MachineSize),


            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Machine>.CreateAsync(machines, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Machine/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .Include(m => m.SalesOrders)
                .FirstOrDefaultAsync(m => m.MachineID == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: Machine/Create
        public IActionResult Create()
        {
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber");
            return View();
        }

        // POST: Machine/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MachineID,SerialNumber,InternalPONumber,MachineSize,MachineClass,MachineSizeDesc,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,SalesOrderID")] Machine machine)
        {
            if (ModelState.IsValid)
            {
                _context.Add(machine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber", machine.SalesOrders);
            return View(machine);
        }

        // GET: Machine/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber", machine.SalesOrders);
            return View(machine);
        }

        // POST: Machine/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MachineID,SerialNumber,InternalPONumber,MachineSize,MachineClass,MachineSizeDesc,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,SalesOrderID")] Machine machine)
        {
            if (id != machine.MachineID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    
                    _context.Update(machine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineExists(machine.MachineID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            } else
            {
                ModelState.AddModelError("", "Your changes are invalid. Please double check them and try again.");
            }
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber", machine.SalesOrders);
            return View(machine);
        }

        // GET: Machine/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .Include(m => m.SalesOrders)
                .FirstOrDefaultAsync(m => m.MachineID == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // POST: Machine/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine != null)
            {
                _context.Machines.Remove(machine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.MachineID == id);
        }
    }
}
