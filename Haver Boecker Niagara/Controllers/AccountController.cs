using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Haver_Boecker_Niagara.Utilities;
using Haver_Boecker_Niagara.CustomControllers;
using Haver_Boecker_Niagara.Models;

namespace Haver_Boecker_Niagara.Controllers
{
    [Authorize(Roles = "admin")]
    public class AccountController : ElephantController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? searchName, string? searchEmail, int? page, int? pageSizeID, string? actionButton, string sortDirection = "asc", string sortField = "Username")
        {
            string[] sortOptions = { "Username", "Email" };
            int filterCount = 0; 

            var users = _userManager.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(searchName))
            {
                users = users.Where(u => EF.Functions.Like(u.UserName, $"%{searchName}%"));
                filterCount++;
            }
            if (!string.IsNullOrEmpty(searchEmail))
            {
                users = users.Where(u => EF.Functions.Like(u.Email, $"%{searchEmail}%"));
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

            users = sortField switch
            {
                "Username" => sortDirection == "asc" ? users.OrderBy(u => u.UserName) : users.OrderByDescending(u => u.UserName),
                "Email" => sortDirection == "asc" ? users.OrderBy(u => u.Email) : users.OrderByDescending(u => u.Email),
                _ => sortDirection == "asc" ? users.OrderBy(u => u.UserName) : users.OrderByDescending(u => u.UserName),
            };

            ViewData["SortField"] = sortField;
            ViewData["SortDirection"] = sortDirection;

            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["PageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData =  await PaginatedList<IdentityUser>.CreateAsync(users, page ?? 1, pageSize);

            return View(pagedData);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Email,UserName")] IdentityUser user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                existingUser.Email = user.Email;
                existingUser.UserName = user.UserName;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }
    }
}