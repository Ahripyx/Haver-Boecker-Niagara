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
    public class KickoffMeetingsController : Controller
    {
        private readonly HaverContext _context;

        public KickoffMeetingsController(HaverContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.GanttSchedules = new SelectList(await _context.GanttSchedules.ToListAsync(), "GanttID", "OrderNumber");
            return PartialView("_komModal", new KickoffMeeting());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KickoffMeeting kickoffMeeting)
        {
            if (ModelState.IsValid)
            {
                _context.KickoffMeetings.Add(kickoffMeeting);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

    }
}