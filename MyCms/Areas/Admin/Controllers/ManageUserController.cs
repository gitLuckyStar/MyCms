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
    public class ManageUserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManageUserController(UserManager<IdentityUser> userManager,
                                    RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            var model = _userManager.Users.Select(u => new IndexViewModel()
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                IsEmailConfirmed = u.EmailConfirmed

            }).ToList();

            return View(model);
        }
        #endregion

        #region EditUser
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            return View(user);
        }
        #endregion

        #region EditUser [HttpPost]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditUser(string id, string username, RoleForUserViewModel model)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(username))
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            user.UserName = username;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(result);
        }
        #endregion

        #region AddRoleForUser
        [HttpGet]
        public async Task<IActionResult> AddRoleForUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            var roles = _roleManager.Roles.ToList();
            var model = new RoleForUserViewModel() { UserId = id };

            foreach (var role in roles)
            {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.UserRoles.Add(new UserRolesViewModel()
                    {
                        RoleName = role.Name
                    });

                }
            }
            return View(model);
        }
        #endregion

        #region AddRoleForUser [HttpPost]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddRoleForUser(RoleForUserViewModel model)
        {
            if (model == null)
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManager.AddToRolesAsync(user, requestRoles);

            if (result.Succeeded)
            {
                return RedirectToAction("index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        #endregion

        #region RemoveRoleFromUser
        [HttpGet]
        public async Task<IActionResult> RemoveRoleFromUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            var roles = _roleManager.Roles.ToList();
            var model = new RoleForUserViewModel() { UserId = id };

            foreach (var role in roles)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.UserRoles.Add(new UserRolesViewModel()
                    {
                        RoleName = role.Name
                    });
                }
            }

            return View(model);
        }
        #endregion

        #region RemoveRoleFromUser
        [HttpPost]
        public async Task<IActionResult> RemoveRoleFromUser(RoleForUserViewModel model)
        {
            if (model == null)
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            var requestRoles = model.UserRoles.Where(r => r.IsSelected)
                .Select(u => u.RoleName)
                .ToList();
            var result = await _userManager.RemoveFromRolesAsync(user, requestRoles);

            if (result.Succeeded) return RedirectToAction("index");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        #endregion

        #region DeleteUser
        [HttpGet]
        public async Task< IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            return View(user);
        }
        #endregion

        #region DeleteUser [HttpPost]
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Error 404  NotFound");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("Error 404  NotFound");
            }
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
