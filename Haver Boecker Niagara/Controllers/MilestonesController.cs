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
    public class MilestonesController : Controller
    {
        private readonly HaverContext _context;

        public MilestonesController(HaverContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int meetingId)
        {
            var kickoffMeeting = await _context.KickoffMeetings
                .Include(k => k.GanttSchedule)
                .FirstOrDefaultAsync(k => k.MeetingID == meetingId);

            if (kickoffMeeting == null)
            {
                return NotFound();
            }

            ViewBag.MeetingID = meetingId;

            var taskNames = Enum.GetValues(typeof(Models.Task)).Cast<Models.Task>();

            if (kickoffMeeting.GanttSchedule?.EngineeringOnly == true)
            {
                taskNames = taskNames.Where(t => t == Models.Task.EngineeringReleased);
            }

            ViewBag.TaskNames = new SelectList(taskNames);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(Status)).Cast<Status>());

            return PartialView("_milestoneModal", new Milestone { KickOfMeetingID = meetingId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                _context.Milestones.Add(milestone);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
    }
}
