using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using Microsoft.AspNetCore.Authorization;

namespace MyCms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Owner")]
    [Area("Admin")]
    public class PageGroupsController : Controller
    {
        private readonly IGroupRepository _GroupRepository;

        public PageGroupsController(IGroupRepository groupRepository)
        {
            _GroupRepository = groupRepository;
        }

        #region Index
        // GET: Admin/PageGroups
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() =>
            {
                return View(_GroupRepository.GetAllGroups());
            });           
        }
        #endregion

        #region Create
        // GET: Admin/PageGroups/Create
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region Create [HttpPost]
        // POST: Admin/PageGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupID,GroupName")] Group pageGroup)
        {
            if (ModelState.IsValid)
            {
                return await Task.Run(() => 
                {
                    _GroupRepository.InsertGroup(pageGroup);
                    _GroupRepository.save();
                    return RedirectToAction(nameof(Index));
                });

            }
            return View(pageGroup);
        }
        #endregion

        #region Edit
        // GET: Admin/PageGroups/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Group Group =_GroupRepository.GetGroupById(id.Value);
            if (Group == null)
            {
                return NotFound();
            }
            return View(Group);
        }
        #endregion

        #region Edit [HttpPost]
        // POST: Admin/PageGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupID,GroupName")] Group Group)
        {
            if (id != Group.GroupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await Task.Run(() =>
                {
                    _GroupRepository.UpdateGroup(Group);
                    _GroupRepository.save();
                    return RedirectToAction(nameof(Index));
                });
            }
            return View(Group);
        }
        #endregion

        #region Delete
        // GET: Admin/PageGroups/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Group Group = _GroupRepository.GetGroupById(id.Value);

            if (Group == null)
            {
                return NotFound();
            }

            return View(Group);
        }
        #endregion

        #region DeleteConfirmed HttpPost
        // POST: Admin/PageGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _GroupRepository.DeleteGroup(id);
            _GroupRepository.save();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _GroupRepository.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
