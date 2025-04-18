﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.Utilities;
using Haver_Boecker_Niagara.CustomControllers;
using Microsoft.AspNetCore.Authorization;

namespace Haver_Boecker_Niagara.Controllers
{
    [Authorize(Roles = "admin,sales,production")]
    public class VendorsController : ElephantController
    {
        private readonly HaverContext _context;

        public VendorsController(HaverContext context)
        {
            _context = context;
        }

        // GET: Vendors
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> Index(string? searchName, string? searchContact, string? searchPhone, string? searchEmail, int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "Name")
        {
            string[] sortOptions = { "Name", "ContactPerson", "PhoneNumber", "Email", "Address", "City", "State", "Country", "PostalCode" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            var vendors = _context.Vendors.AsNoTracking();

            if (!string.IsNullOrEmpty(searchName))
            {
                vendors = vendors.Where(v => EF.Functions.Like(v.Name, $"%{searchName}%"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchContact))
            {
                var nameParts = searchContact.Split(' ');

                if (nameParts.Length == 1)
                {
                    vendors = vendors.Where(c => EF.Functions.Like(c.ContactFirstName, $"%{searchContact}%")
                                                  || EF.Functions.Like(c.ContactLastName, $"%{searchContact}%"));
                }
                else if (nameParts.Length >= 2)
                {
                    vendors = vendors.Where(c => EF.Functions.Like(c.ContactFirstName, $"%{nameParts[0]}%")
                                                  && EF.Functions.Like(c.ContactLastName, $"%{nameParts[1]}%"));
                }
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                vendors = vendors.Where(v => EF.Functions.Like(v.Email, $"%{searchEmail}%"));
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
            vendors = sortField switch
            {
                "Name" => sortDirection == "asc" ? vendors.OrderBy(c => c.Name) : vendors.OrderByDescending(c => c.Name),
                "ContactPerson" => sortDirection == "asc"
                    ? vendors.OrderBy(c => c.ContactFirstName).ThenBy(c => c.ContactLastName)
                    : vendors.OrderByDescending(c => c.ContactFirstName).ThenByDescending(c => c.ContactLastName),
                "Email" => sortDirection == "asc" ? vendors.OrderBy(v => v.Email) : vendors.OrderByDescending(v => v.Email),
                _ => sortDirection == "asc" ? vendors.OrderBy(v => v.Name) : vendors.OrderByDescending(v => v.Name),
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Vendor>.CreateAsync(vendors, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Vendors/Details/5
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vendor = await _context.Vendors.AsNoTracking().FirstOrDefaultAsync(m => m.VendorID == id);
            if (vendor == null) return NotFound();

            return View(vendor);
        }
        // GET: Vendotr/Create
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> Create([Bind("VendorID,Name,ContactFirstName,ContactLastName,PhoneNumber,Email,Address,City,Country,PostalCode,CreatedAt,UpdatedAt")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                vendor.CreatedAt = DateTime.UtcNow;
                vendor.UpdatedAt = DateTime.UtcNow;
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }


        // GET: Vendors/Edit/5
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vendor = await _context.Vendors.FindAsync(id);
            return vendor == null ? NotFound() : View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> Edit(int id, [Bind("VendorID,Name,ContactFirstName,ContactLastName,PhoneNumber,Email,Address,City,Country,PostalCode,CreatedAt,UpdatedAt")] Vendor vendor)
        {
            if (id != vendor.VendorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    vendor.UpdatedAt = DateTime.UtcNow;
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vendors.Any(e => e.VendorID == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vendor = await _context.Vendors.AsNoTracking().FirstOrDefaultAsync(m => m.VendorID == id);
            if (vendor == null) return NotFound();

            return View(vendor);
        }

        // POST: Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,sales,production")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor != null)
            {
                _context.Vendors.Remove(vendor);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
