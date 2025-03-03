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

        // GET: Milestones
        public async Task<IActionResult> Index()
        {
            var haverContext = _context.Milestones.Include(m => m.KickoffMeeting);
            return View(await haverContext.ToListAsync());
        }

        // GET: Milestones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestone = await _context.Milestones
                .Include(m => m.KickoffMeeting)
                .FirstOrDefaultAsync(m => m.MilestoneID == id);
            if (milestone == null)
            {
                return NotFound();
            }

            return View(milestone);
        }

        // GET: Milestones/Create
        public IActionResult Create()
        {
            ViewData["KickOfMeetingID"] = new SelectList(_context.KickoffMeetings, "MeetingID", "MeetingID");
            return View();
        }

        // POST: Milestones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MilestoneID,Name,KickOfMeetingID,StartDate,EndDate,ActualCompletionDate,Status")] Milestone milestone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(milestone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KickOfMeetingID"] = new SelectList(_context.KickoffMeetings, "MeetingID", "MeetingID", milestone.KickOfMeetingID);
            return View(milestone);
        }

        // GET: Milestones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestone = await _context.Milestones.FindAsync(id);
            if (milestone == null)
            {
                return NotFound();
            }
            ViewData["KickOfMeetingID"] = new SelectList(_context.KickoffMeetings, "MeetingID", "MeetingID", milestone.KickOfMeetingID);
            return View(milestone);
        }

        // POST: Milestones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MilestoneID,Name,KickOfMeetingID,StartDate,EndDate,ActualCompletionDate,Status")] Milestone milestone)
        {
            if (id != milestone.MilestoneID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(milestone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MilestoneExists(milestone.MilestoneID))
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
            ViewData["KickOfMeetingID"] = new SelectList(_context.KickoffMeetings, "MeetingID", "MeetingID", milestone.KickOfMeetingID);
            return View(milestone);
        }

        // GET: Milestones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var milestone = await _context.Milestones
                .Include(m => m.KickoffMeeting)
                .FirstOrDefaultAsync(m => m.MilestoneID == id);
            if (milestone == null)
            {
                return NotFound();
            }

            return View(milestone);
        }

        // POST: Milestones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var milestone = await _context.Milestones.FindAsync(id);
            if (milestone != null)
            {
                _context.Milestones.Remove(milestone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MilestoneExists(int id)
        {
            return _context.Milestones.Any(e => e.MilestoneID == id);
        }
    }
}
