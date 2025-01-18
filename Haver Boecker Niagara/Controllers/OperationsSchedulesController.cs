using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;

namespace Haver_Boecker_Niagara.Controllers
{
    public class OperationsSchedulesController : Controller
    {
        private readonly HaverContext _context;

        public OperationsSchedulesController(HaverContext context)
        {
            _context = context;
        }

        // GET: OperationsSchedules
        public async Task<IActionResult> Index()
        {
            var haverContext = _context.OperationsSchedules.Include(o => o.Customer).Include(o => o.Vendor);
            return View(await haverContext.ToListAsync());
        }

        // GET: OperationsSchedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationsSchedule = await _context.OperationsSchedules
                .Include(o => o.Customer)
                .Include(o => o.Vendor)
                .FirstOrDefaultAsync(m => m.OperationsID == id);
            if (operationsSchedule == null)
            {
                return NotFound();
            }

            return View(operationsSchedule);
        }

        // GET: OperationsSchedules/Create
        public IActionResult Create()
        {
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID");
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "VendorID");
            return View();
        }

        // POST: OperationsSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationsID,SalesOrder,CustomerID,VendorID,MachineDescription,SerialNumber,PackageReleaseDate,PurchaseOrderNumber,PODueDate,DeliveryDate,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,Notes")] OperationsSchedule operationsSchedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(operationsSchedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", operationsSchedule.CustomerID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "VendorID", operationsSchedule.VendorID);
            return View(operationsSchedule);
        }

        // GET: OperationsSchedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var operationsSchedule = await _context.OperationsSchedules.FindAsync(id);
            if (operationsSchedule == null)
            {
                return NotFound();
            }
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", operationsSchedule.CustomerID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "VendorID", operationsSchedule.VendorID);
            return View(operationsSchedule);
        }

        // POST: OperationsSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OperationsID,SalesOrder,CustomerID,VendorID,MachineDescription,SerialNumber,PackageReleaseDate,PurchaseOrderNumber,PODueDate,DeliveryDate,Media,SparePartsMedia,Base,AirSeal,CoatingOrLining,Disassembly,Notes")] OperationsSchedule operationsSchedule)
        {
            if (id != operationsSchedule.OperationsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operationsSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationsScheduleExists(operationsSchedule.OperationsID))
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
            ViewData["CustomerID"] = new SelectList(_context.Customers, "CustomerID", "CustomerID", operationsSchedule.CustomerID);
            ViewData["VendorID"] = new SelectList(_context.Vendors, "VendorID", "VendorID", operationsSchedule.VendorID);
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
                .Include(o => o.Customer)
                .Include(o => o.Vendor)
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
