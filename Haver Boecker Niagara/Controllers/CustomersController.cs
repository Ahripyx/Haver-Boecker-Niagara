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
        public async Task<IActionResult> Index(string? SearchName, string? SearchContact,
            string? SearchPhone, string? SearchEmail, int? page, int? pageSizeID, string? actionButton,
            string sortDirection = "asc", string sortField = "Customer")
        {
            //List of sort options
            string[] sortOptions = new[] { "Name", "ContactPerson", "PhoneNumber", "Email" };

            //Number of filters applied
            ViewData["Filtering"] = "btn-outline-secondary";
            int numberFilters = 0;

            var customers = _context.Customers
                .AsNoTracking();

            if (!String.IsNullOrEmpty(SearchName))
            {
                customers = customers.Where( c => c.Name.Contains(SearchName));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchContact))
            {
                customers = customers.Where( c=> c.ContactPerson.Contains(SearchContact));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchPhone))
            {
                customers = customers.Where( c => c.PhoneNumber.Contains(SearchPhone));
                numberFilters++;
            }
            if (!String.IsNullOrEmpty(SearchEmail))
            {
                customers = customers.Where( c => c.Email.Contains(SearchEmail));
                numberFilters++;
            }
            //Give feedback about the state of the filters
            if (numberFilters != 0)
            {
                //Toggle the Open/Closed state of the collapse depending on if we are filtering
                ViewData["Filtering"] = " btn-danger";
                //Show how many filters have been applied
                ViewData["numberFilters"] = "(" + numberFilters.ToString()
                    + " Filter" + (numberFilters > 1 ? "s" : "") + " Applied)";
                //Keep the Bootstrap collapse open
                @ViewData["ShowFilter"] = " show";

            }

            //See if we have called for a change of filtering or sorting
            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;//Reset page to start

                if (sortOptions.Contains(actionButton))//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = sortDirection == "asc" ? "desc" : "asc";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

            //Handling sorting
            if (sortField == "Name")
            {
                if (sortDirection == "asc")
                {
                    customers = customers
                        .OrderBy(c => c.Name);
                }
                else
                {
                    customers = customers
                        .OrderByDescending(c => c.Name);
                }
            }
            else if (sortField == "ContactPerson")
            {
                if (sortDirection == "asc")
                {
                    customers = customers
                        .OrderBy(c => c.Email);
                }
                else
                {
                    customers = customers
                        .OrderByDescending(c => c.Email);
                }
            }
            else if (sortField == "PhoneNumber")
            {
                if (sortDirection == "asc")
                {
                    customers = customers
                        .OrderBy(c => c.PhoneNumber);
                }
                else
                {
                    customers = customers
                        .OrderByDescending(c => c.PhoneNumber);
                }
            }
            else //Sorting by email
            {
                if (sortDirection == "asc")
                {
                    customers = customers
                        .OrderBy(c => c.Email);
                }
                else
                {
                    customers = customers
                        .OrderByDescending(c => c.Email);
                }
            }
            //Setting sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<Customer>.CreateAsync(customers.AsNoTracking(), page ?? 1, pageSize);

            return View(pagedData);
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerID,Name,ContactPerson,PhoneNumber,Email,Address,City,State,Country,PostalCode,Description")] Customer customer)
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
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerID,Name,ContactPerson,PhoneNumber,Email,Address,City,State,Country,PostalCode,Description")] Customer customer)
        {
            if (id != customer.CustomerID)
            {
                return NotFound();
            }

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
                    if (!CustomerExists(customer.CustomerID))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CustomerID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
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

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
