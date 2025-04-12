using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.Utilities;
using Haver_Boecker_Niagara.CustomControllers;
using Microsoft.AspNetCore.Authorization;
using Haver_Boecker_Niagara.Future_Models;

namespace Haver_Boecker_Niagara.Controllers
{
    [Authorize(Roles = "admin,production")]
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
                ganttSchedules = ganttSchedules.Where(g => g.SalesOrder.OrderNumber.ToLower().Contains(searchOrderNo.ToLower()));
            }

            if (!string.IsNullOrEmpty(searchCustomer))
            {
                ganttSchedules = ganttSchedules.Where(g => g.SalesOrder.Customer.Name.ToLower().Contains(searchCustomer.ToLower()));
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


            ganttSchedule.KickoffMeetings = ganttSchedule.KickoffMeetings?
            .OrderByDescending(k => k.MeetingDate)
            .Take(3)
            .ToList();
            var milestoneStatus = latestMilestone?.Status;

            ganttSchedule.LatestMilestone = latestMilestone != null
            ? $"{latestMilestone.Name} ({latestMilestone.Status})"
            : "No Milestones";

            ViewData["MilestoneStatus"] = latestMilestone?.Status;

            return View(ganttSchedule);
        }
        [Authorize(Roles = "admin,production")]

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
        [Authorize(Roles = "admin,production")]

        public async Task<IActionResult> Edit(int id, [Bind("GanttID,SalesOrderID,MachineID, PreOrdersExpected,ReadinessToShipExpected,PromiseDate,DeadlineDate,NCR,EngineeringOnly")] GanttSchedule ganttSchedule)
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
        [Authorize(Roles = "admin,production")]

        public IActionResult Create()
        {
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrders, "SalesOrderID", "OrderNumber");
            return View();
        }
        [Authorize(Roles = "admin,production")]

        public async Task<IActionResult> CreateKickoffMeeting([Bind("GanttID,MeetingSummary")] KickoffMeeting kickoffMeeting)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kickoffMeeting);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Details", new {ID = kickoffMeeting.GanttID});
        }
        [Authorize(Roles = "admin,production")]

        public async Task<IActionResult> CreateMilestone([Bind("Name,KickOfMeetingID,EndDate")] Milestone milestone)
        {
            milestone.StartDate = DateTime.Today;
            milestone.Status = Status.Open;
            if (ModelState.IsValid)
            {
                _context.Add(milestone);
                await _context.SaveChangesAsync();
            }
            var redirectID = _context.KickoffMeetings.Find(milestone.KickOfMeetingID).GanttID;
            return RedirectToAction("Details", new {ID = redirectID});
        }
        [Authorize(Roles = "admin,production")]
        [HttpGet]
        [Produces("application/json")]
        [Route("GanttSchedules/GetTasks/{id}")] 
        public async Task<IActionResult> GetTasks(int id)
        {
            try
            {
                // Verify access to the schedule
                var hasAccess = await _context.GanttSchedules
                    .AnyAsync(g => g.GanttID == id);

                if (!hasAccess)
                {
                    Response.ContentType = "application/json";
                    return Json(new { error = "Access denied" });
                }

                // Get tasks with null checks
                var tasks = await _context.GanttSchedules
                    .Where(g => g.GanttID == id)
                    .SelectMany(g => g.KickoffMeetings)
                    .SelectMany(km => km.Milestones)
                    .Select(m => new
                    {
                        id = m.MilestoneID,
                        name = m.Name.ToString(),
                        start = m.StartDate.HasValue ?
                    m.StartDate.Value.ToString("yyyy-MM-dd") :
                    DateTime.Today.ToString("yyyy-MM-dd"),
                        end = m.EndDate.HasValue ?
                    m.EndDate.Value.ToString("yyyy-MM-dd") :
                    DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                        progress = m.Status == Status.Closed ? 100 : 0,
                        dependencies = ""
                    })
                    .ToListAsync();

                Response.ContentType = "application/json";
                return Json(tasks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask(TaskUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var milestone = await _context.Milestones.FindAsync(model.ID);
                if (milestone == null)
                {
                    return NotFound($"Milestone with ID {model.ID} not found");
                }

                if (DateTime.TryParse(model.StartDate, out var startDate))
                {
                    milestone.StartDate = startDate;
                }

                if (DateTime.TryParse(model.EndDate, out var endDate))
                {
                    milestone.EndDate = endDate;
                }

                milestone.Status = model.Progress >= 100 ? Status.Closed : Status.Open;

                _context.Update(milestone);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Task updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating task: {ex.Message}");
            }
        }
        public async Task<IActionResult> GetMilestoneModal(int id)
        {
            return PartialView("_milestoneModal", new Milestone {KickOfMeetingID = id});
        }

    }
}