using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.Utilities;
using Haver_Boecker_Niagara.ViewModels;
using Haver_Boecker_Niagara.CustomControllers;

namespace Haver_Boecker_Niagara.Controllers
{
    public class GanttSchedulesController : ElephantController
    {
        private readonly HaverContext _context;

        public GanttSchedulesController(HaverContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            string? searchOrderNo,
            string? searchCustomer,
            int? page,
            int? pageSizeID,
            string? actionButton,
            string sortDirection = "asc",
            string sortField = "OrderNumber")
        {
            var ganttSchedules = _context.GanttSchedules
                .Include(g => g.SalesOrder)
                .ThenInclude(s => s.Customer)
                .Include(g => g.SalesOrder.EngineeringPackage)
                .ThenInclude(ep => ep.Engineers)
                .Include(g => g.SalesOrder.Machines)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchOrderNo))
            {
                ganttSchedules = ganttSchedules.Where(g => g.SalesOrder.OrderNumber.Contains(searchOrderNo));
            }

            if (!string.IsNullOrEmpty(searchCustomer))
            {
                ganttSchedules = ganttSchedules.Where(g => g.SalesOrder.Customer.Name.Contains(searchCustomer));
            }

            ganttSchedules = sortField switch
            {
                "OrderNumber" => sortDirection == "asc" ?
                    ganttSchedules.OrderBy(g => g.SalesOrder.OrderNumber) :
                    ganttSchedules.OrderByDescending(g => g.SalesOrder.OrderNumber),
                _ => ganttSchedules
            };
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<GanttSchedule>.CreateAsync(ganttSchedules, page ?? 1, pageSize);

            return View(pagedData);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var ganttSchedule = await _context.GanttSchedules
                .Include(g => g.SalesOrder)
                .ThenInclude(s => s.Customer)
                .Include(g => g.SalesOrder.EngineeringPackage)
                .ThenInclude(ep => ep.Engineers)
                .Include(g => g.SalesOrder.Machines)
                .Include(g => g.KickoffMeetings) 
                .ThenInclude(k => k.Milestones)
                .FirstOrDefaultAsync(m => m.GanttID == id);

            if (ganttSchedule == null)
                return NotFound();

            var latestMilestone = ganttSchedule.KickoffMeetings?
             .SelectMany(k => k.Milestones)
             .OrderByDescending(m => m.MilestoneID)
             .FirstOrDefault();

            var milestoneStatus = latestMilestone?.Status;

            ganttSchedule.LatestMilestone = latestMilestone != null
            ? $"{latestMilestone.Name} ({latestMilestone.Status})"
            : "No Milestones";

            ViewData["MilestoneStatus"] = latestMilestone?.Status;
 


            return View(ganttSchedule);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var ganttSchedule = await _context.GanttSchedules
                .Include(g => g.SalesOrder)
                .FirstOrDefaultAsync(m => m.GanttID == id);

            if (ganttSchedule == null)
                return NotFound();

            return View(ganttSchedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GanttID,SalesOrderID,PreOrdersExpected,ReadinessToShipExpected,PromiseDate,DeadlineDate,NCR,EngineeringOnly")] GanttSchedule ganttSchedule)
        {
            if (id != ganttSchedule.GanttID)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ganttSchedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.GanttSchedules.Any(e => e.GanttID == ganttSchedule.GanttID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ganttSchedule);
        }

        public IActionResult Create()
        {
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("SalesOrderID,EngineeringOnly,PreOrdersExpected,ReadinessToShipExpected,PromiseDate,DeadlineDate,NCR")] GanttSchedule ganttSchedule)
        {
            if (ModelState.IsValid)
            {
                var salesOrder = await _context.SalesOrders
                    .Include(s => s.Machines)
                    .FirstOrDefaultAsync(s => s.SalesOrderID == ganttSchedule.SalesOrderID);

                if (salesOrder == null)
                    return NotFound();

                if (ganttSchedule.EngineeringOnly)
                {
                    var newSchedule = new GanttSchedule
                    {
                        SalesOrderID = salesOrder.SalesOrderID,
                        EngineeringOnly = true,
                        PreOrdersExpected = ganttSchedule.PreOrdersExpected,
                        ReadinessToShipExpected = ganttSchedule.ReadinessToShipExpected,
                        PromiseDate = ganttSchedule.PromiseDate,
                        DeadlineDate = ganttSchedule.DeadlineDate,
                        NCR = ganttSchedule.NCR
                    };
                    _context.GanttSchedules.Add(newSchedule);
                }
                else
                {
                    var groupedMachines = salesOrder.Machines.GroupBy(m => m.MachineDescription);

                    foreach (var group in groupedMachines)
                    {
                        var newSchedule = new GanttSchedule
                        {
                            SalesOrderID = salesOrder.SalesOrderID,
                            MachineID = group.First().MachineID,
                            EngineeringOnly = false,
                            PreOrdersExpected = ganttSchedule.PreOrdersExpected,
                            ReadinessToShipExpected = ganttSchedule.ReadinessToShipExpected,
                            PromiseDate = ganttSchedule.PromiseDate,
                            DeadlineDate = ganttSchedule.DeadlineDate,
                            NCR = ganttSchedule.NCR
                        };
                        _context.GanttSchedules.Add(newSchedule);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber", ganttSchedule.SalesOrderID);
            return View(ganttSchedule);
        }
    }
}