using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Data;
using Haver_Boecker_Niagara.Models;
using Haver_Boecker_Niagara.Utilities;
using Haver_Boecker_Niagara.CustomControllers;

namespace Haver_Boecker_Niagara.Controllers
{
    public class CustomersController : ElephantController
    {
        private readonly HaverContext _context;

        public CustomersController(HaverContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string? searchName, string? searchContact, string? searchPhone, string? searchEmail, int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "Name")
        {
            string[] sortOptions = { "Name", "ContactPerson", "PhoneNumber", "Email", "Address", "City", "State", "Country", "PostalCode" };
            ViewData["Filtering"] = "btn-outline-secondary";
            int filterCount = 0;

            var customers = _context.Customers.AsNoTracking();

            if (!string.IsNullOrEmpty(searchName))
            {
                customers = customers.Where(c => EF.Functions.Like(c.Name, $"%{searchName}%"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchContact))
            {
                customers = customers.Where(c => EF.Functions.Like(c.ContactPerson, $"%{searchContact}%"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchPhone))
            {
                customers = customers.Where(c => EF.Functions.Like(c.PhoneNumber, $"%{searchPhone}%"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                customers = customers.Where(c => EF.Functions.Like(c.Email, $"%{searchEmail}%"));
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

            customers = sortField switch
            {
                "Name" => sortDirection == "asc" ? customers.OrderBy(c => c.Name) : customers.OrderByDescending(c => c.Name),
                "ContactPerson" => sortDirection == "asc" ? customers.OrderBy(c => c.ContactPerson) : customers.OrderByDescending(c => c.ContactPerson),
                "PhoneNumber" => sortDirection == "asc" ? customers.OrderBy(c => c.PhoneNumber) : customers.OrderByDescending(c => c.PhoneNumber),
                "Email" => sortDirection == "asc" ? customers.OrderBy(c => c.Email) : customers.OrderByDescending(c => c.Email),
                "Address" => sortDirection == "asc" ? customers.OrderBy(c => c.Address) : customers.OrderByDescending(c => c.Address),
                "City" => sortDirection == "asc" ? customers.OrderBy(c => c.City) : customers.OrderByDescending(c => c.City),
                "State" => sortDirection == "asc" ? customers.OrderBy(c => c.State) : customers.OrderByDescending(c => c.State),
                "Country" => sortDirection == "asc" ? customers.OrderBy(c => c.Country) : customers.OrderByDescending(c => c.Country),
                "PostalCode" => sortDirection == "asc" ? customers.OrderBy(c => c.PostalCode) : customers.OrderByDescending(c => c.PostalCode),
                _ => sortDirection == "asc" ? customers.OrderBy(c => c.Name) : customers.OrderByDescending(c => c.Name),
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Customer>.CreateAsync(customers, page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(m => m.CustomerID == id);
            return customer == null ? NotFound() : View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create() => View();

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,Name,ContactPerson,PhoneNumber,Email,Address,City,State,Country,PostalCode")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.CreatedAt = DateTime.UtcNow;
                customer.UpdatedAt = DateTime.UtcNow;

                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.FindAsync(id);
            return customer == null ? NotFound() : View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Name,ContactPerson,PhoneNumber,Email,Address,City,State,Country,PostalCode")] Customer customer)
        {
            if (id != customer.CustomerID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    customer.UpdatedAt = DateTime.UtcNow;
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customers.Any(e => e.CustomerID == id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(m => m.CustomerID == id);
            return customer == null ? NotFound() : View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
