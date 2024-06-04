using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Owner")]
    [Area("Admin")]
    public class ManageRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageRoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        #region Index
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        #endregion

        #region AddRole
        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }
        #endregion

        #region AddRole [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound("Error 404  NotFound");
            }

            var role = new IdentityRole(name);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(role);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Error 404  NotFound");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound("Error 404  NotFound");
            }
            return View(role);
        }
        #endregion

        #region Delete [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string itemid)
        {
            if (string.IsNullOrEmpty(itemid))
            {
                return NotFound("Error 404  NotFound");
            }
            var role = await _roleManager.FindByIdAsync(itemid);
            if (role == null)
            {
                return NotFound("Error 404  NotFound");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(role);
        }
        #endregion

    }
}
