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
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Reflection.PortableExecutable;

namespace Haver_Boecker_Niagara.Controllers
{
    public class EngineeringPackageController : ElephantController
    {
        private readonly HaverContext _context;

        public EngineeringPackageController(HaverContext context)
        {
            _context = context;
        }

        // GET: EngineeringPackage
        public async Task<IActionResult> Index
        (
            DateTime? startDate,
            DateTime? endDate,
            string? FilterCriteria,
            int? page,
            int? pageSizeID,
            string? actionButton,
            string sortDirection = "asc",
            string sortField = "ActualAprv"
        ) { 
            string[] sortOptions = { "EstimatedRel", "EstimatedAprv", "ActualRel", "ActualAprv" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            string[] filterCriterias = 
            { 
                "None", 
                "Estimated Release Date", 
                "Estimated Approval Date", 
                "Actual Release Date", 
                "Actual Approval Date" 
            };
            if (FilterCriteria == null || ViewData["FilterCriteria"] == null) {
      
               ViewData["FilterCriteria"] = new SelectList(filterCriterias, null);
            } else
            {
               ViewData["SelectedCriteria"] = FilterCriteria;
            }

            if (startDate == null)
            {
                startDate = DateTime.MinValue;
            } else
            {
                ViewData["startDate"] = startDate;
            }
            if (endDate == null)
            {
                endDate = DateTime.MaxValue;
            } else
            {
                ViewData["endDate"] = endDate;
            }

            if (startDate > endDate)
            {
                startDate = endDate;
            }

            var engPackages = _context
                              .EngineeringPackages
                              .Include(p => p.Engineers)
                              .AsTracking();

            if (!String.IsNullOrEmpty(FilterCriteria) && FilterCriteria != "None")
            {
                filterCount++;
            }


            engPackages = (FilterCriteria) switch
            {
                "Estimated Release Date" => 
                    engPackages.Where(p => p.PackageReleaseDate.Value >= startDate 
                                        && p.PackageReleaseDate.Value <= endDate),
                
                "Estimated Approval Date" => 
                    engPackages.Where(p => p.ApprovalDrawingDate.Value >= startDate 
                                        && p.ApprovalDrawingDate.Value <= endDate),
                
                "Actual Release Date" => 
                    engPackages.Where(p => p.ActualPackageReleaseDate.Value >= startDate 
                                        && p.ActualPackageReleaseDate.Value <= endDate),
                
                "Actual Approval Date" => 
                    engPackages.Where(p => p.ActualApprovalDrawingDate.Value >= startDate 
                                        && p.ActualApprovalDrawingDate.Value <= endDate),
                
                _ => engPackages
            };

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

            engPackages = sortField switch
            {
                "EstimatedRel" => sortDirection == "asc" 
                ? engPackages.OrderBy(c => c.PackageReleaseDate) 
                : engPackages.OrderByDescending(c => c.PackageReleaseDate),

                "EstimatedAprv" => sortDirection == "asc" 
                ? engPackages.OrderBy(c => c.ApprovalDrawingDate) 
                : engPackages.OrderByDescending(c => c.ApprovalDrawingDate),

                "ActualRel" => sortDirection == "asc" 
                ? engPackages.OrderBy(c => c.ActualPackageReleaseDate) 
                : engPackages.OrderByDescending(c => c.ActualPackageReleaseDate),

                "ActualAprv" => sortDirection == "asc" 
                ? engPackages.OrderBy(c => c.ActualApprovalDrawingDate) 
                : engPackages.OrderByDescending(c => c.ActualApprovalDrawingDate),

                _ => engPackages
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;


            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<EngineeringPackage>.CreateAsync(engPackages, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: EngineeringPackage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineeringPackage = await _context.EngineeringPackages
                .Include(p => p.Engineers)
                .FirstOrDefaultAsync(m => m.EngineeringPackageID == id);
            if (engineeringPackage == null)
            {
                return NotFound();
            }

            return View(engineeringPackage);
        }

        // GET: EngineeringPackage/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: EngineeringPackage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EngineeringPackageID,PackageReleaseDate,ApprovalDrawingDate,ActualPackageReleaseDate,ActualApprovalDrawingDate")] EngineeringPackage engineeringPackage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(engineeringPackage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Engineers"] = new SelectList(_context.Engineers.Where(p => !engineeringPackage.Engineers.Contains(p)), "EngineerID", "Name");
            return View(engineeringPackage);
        }

        // GET: EngineeringPackage/Edit/5
        public async Task<IActionResult> Edit(int? id, int? setCountEng)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineeringPackage = await _context.EngineeringPackages.Include(p => p.Engineers).Where(p => p.EngineeringPackageID == id).FirstOrDefaultAsync();

            if (setCountEng == null)
            {
                setCountEng = engineeringPackage.Engineers.Count;
                
            }
            ViewData["setCountEng"] = setCountEng;
            ViewData["engCountDD"] = new SelectList(Enumerable.Range(0, 5), setCountEng);
            while (setCountEng > 0)
            {
                if (engineeringPackage.Engineers.Count >= setCountEng)
                {
                    ViewData["engineer" + (setCountEng - 1).ToString()] = new SelectList(_context.Engineers, "EngineerID", "Name", engineeringPackage.Engineers.ElementAt(setCountEng.Value-1).EngineerID);

                } else
                {
                    ViewData["engineer" + (setCountEng - 1).ToString()] = new SelectList(_context.Engineers, "EngineerID", "Name");

                }
                setCountEng--;
            }
            if (engineeringPackage == null)
            {
                return NotFound();
            }
            return View(engineeringPackage);
        }

        // POST: EngineeringPackage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EngineeringPackageID,PackageReleaseDate,ApprovalDrawingDate,ActualPackageReleaseDate,ActualApprovalDrawingDate")] EngineeringPackage engineeringPackage, int? setCountEng, int? engineer0, int? engineer1, int? engineer2, int? engineer3)
        {
            if (id != engineeringPackage.EngineeringPackageID)
            {
                return NotFound();
            }
            

            if (ModelState.IsValid)
            {
                foreach (var engPack in _context.EngineeringPackageEngineers
                .Where(p => p.EngineeringPackageID == engineeringPackage.EngineeringPackageID))
                {
                    _context.Remove(engPack);
                }
                _context.SaveChanges();
                engineeringPackage.Engineers = new List<Engineer>();
                // create new engis
                if (engineer0 != null && setCountEng >= 1)
                {
                    var eng1 = _context.Engineers.Find(engineer0);
                    engineeringPackage.Engineers.Add(eng1);
                }
                if (engineer1 != null && setCountEng >= 2)
                {
                    var eng2 = _context.Engineers.Find(engineer1);
                    engineeringPackage.Engineers.Add(eng2);
                }
                if (engineer2 != null && setCountEng >= 3)
                {
                    var eng3 = _context.Engineers.Find(engineer2);
                    engineeringPackage.Engineers.Add(eng3);
                }
                if (engineer3 != null && setCountEng == 4)
                {
                    var eng4 = _context.Engineers.Find(engineer3);
                    engineeringPackage.Engineers.Add(eng4);
                }
                try
                {
                    _context.Update(engineeringPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EngineeringPackageExists(engineeringPackage.EngineeringPackageID))
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
            return View(engineeringPackage);
        }

        // GET: EngineeringPackage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var engineeringPackage = await _context.EngineeringPackages
                .FirstOrDefaultAsync(m => m.EngineeringPackageID == id);
            if (engineeringPackage == null)
            {
                return NotFound();
            }

            return View(engineeringPackage);
        }

        // POST: EngineeringPackage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var engineeringPackage = await _context.EngineeringPackages.FindAsync(id);
            if (engineeringPackage != null)
            {
                _context.EngineeringPackages.Remove(engineeringPackage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EngineeringPackageExists(int id)
        {
            return _context.EngineeringPackages.Any(e => e.EngineeringPackageID == id);
        }
    }
}
