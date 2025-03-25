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
using Microsoft.AspNetCore.Authorization;

namespace Haver_Boecker_Niagara.Controllers
{
    public class EngineersController : ElephantController
    {
        private readonly HaverContext _context;

        public EngineersController(HaverContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "admin, engineering,read only")]

        // GET: Engineers
        public async Task<IActionResult> Index(string? searchName, string? searchEmail, int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "Name")
        {
            string[] sortOptions = { "Name", "Email" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            var engineers = _context.Engineers.AsNoTracking();

            if (!string.IsNullOrEmpty(searchName))
            {
                var nameParts = searchName.Split(' ');

                if (nameParts.Length == 1)
                {
                    engineers = engineers.Where(e => EF.Functions.Like(e.FirstName, $"%{searchName}%")
                                                  || EF.Functions.Like(e.LastName, $"%{searchName}%"));
                }
                else if (nameParts.Length >= 2)
                {
                    engineers = engineers.Where(e => EF.Functions.Like(e.FirstName, $"%{nameParts[0]}%")
                                                  && EF.Functions.Like(e.LastName, $"%{nameParts[1]}%"));
                }
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                engineers = engineers.Where(e => EF.Functions.Like(e.Email, $"%{searchEmail}%"));
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

            engineers = sortField switch
            {
                "Name" => sortDirection == "asc" ? engineers.OrderBy(e => e.FirstName).ThenBy(e => e.LastName) : engineers.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName),
                "Email" => sortDirection == "asc" ? engineers.OrderBy(e => e.Email) : engineers.OrderByDescending(e => e.Email),
                _ => sortDirection == "asc" ? engineers.OrderBy(e => e.Name) : engineers.OrderByDescending(e => e.Name),
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Engineer>.CreateAsync(engineers, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Engineers/Details/5
        [Authorize(Roles = "admin, engineering,read only")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineer = await _context.Engineers
                .FirstOrDefaultAsync(m => m.EngineerID == id);
            if (engineer == null)
            {
                return NotFound();
            }

            return View(engineer);
        }
        [Authorize(Roles = "admin, engineering")]

        // GET: Engineers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Engineers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, engineering")]

        public async Task<IActionResult> Create([Bind("EngineerID,FirstName,LastName,Email")] Engineer engineer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(engineer);
      
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(engineer);
        }
        [Authorize(Roles = "admin, engineering")]

        // GET: Engineers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineer = await _context.Engineers.FindAsync(id);
            if (engineer == null)
            {
                return NotFound();
            }
            return View(engineer);
        }

        // POST: Engineers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, engineering")]

        public async Task<IActionResult> Edit(int id, [Bind("EngineerID,FirstName,LastName,Email")] Engineer engineer)
        {
            if (id != engineer.EngineerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(engineer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EngineerExists(engineer.EngineerID))
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
            return View(engineer);
        }

        // GET: Engineers/Delete/5
        [Authorize(Roles = "admin, engineering")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineer = await _context.Engineers
                .FirstOrDefaultAsync(m => m.EngineerID == id);
            if (engineer == null)
            {
                return NotFound();
            }

            return View(engineer);
        }

        // POST: Engineers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, engineering")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var engineer = await _context.Engineers.FindAsync(id);
            if (engineer != null)
            {
                _context.Engineers.Remove(engineer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EngineerExists(int id)
        {
            return _context.Engineers.Any(e => e.EngineerID == id);
        }
    }
}
