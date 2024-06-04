using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace MyCms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Owner,Publisher")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private IPageRepository _PageRepository;
        private IGroupRepository _GroupRepository;

        public PagesController(IPageRepository PageRepository,
                               IGroupRepository GroupRepository)
        {
            _PageRepository = PageRepository;
            _GroupRepository = GroupRepository;
        }
        #region Index
        // GET: Admin/Pages
        public IActionResult Index(string publisher)
        {
            if (User.IsInRole("Owner"))
            {
                return View(_PageRepository.GetAllPages());
            }
            publisher = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_PageRepository.GetPublisherPage(publisher));
        }
        #endregion

        #region Create
        // GET: Admin/Pages/Create
        public IActionResult Create()
        {
            ViewBag.GroupID = new SelectList(_GroupRepository.GetAllGroups(), "GroupID", "GroupName");
            return View();
        }
        #endregion

        #region Create [HttpPost]
        // POST: Admin/Pages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("PageID,GroupID,Publisher,PublisherID,PageTitle,ShortDescription,Text,ImageFile,ShowInSlider,View,CreateDate,Tags,ImageName")] Page page)
        {
            ModelState.Remove("Publisher");
            ModelState.Remove("PublisherID");
            if (ModelState.IsValid)
            {
                page.View = 0;
                page.CreateDate = DateTime.Now;
                page.Publisher = User.FindFirstValue(ClaimTypes.Name);
                page.PublisherID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //get file extension
                //FileInfo fileInfo = new FileInfo(page.ImageAddress.FileName);
                if (page.ImageFile != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                    //create folder if not exist
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    string fileName = Guid.NewGuid() + page.ImageFile.FileName;

                    string fileNameWithPath = Path.Combine(path, fileName);

                    using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                    page.ImageFile.CopyTo(stream);
                    page.ImageName = fileName;
                }
                _PageRepository.InsertPage(page);
                _PageRepository.save();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Create));
        }
        #endregion

        #region Edit
        // GET: Admin/Pages/Edit/5
        public IActionResult Edit(int? id)
        {
            Page page = _PageRepository.GetPagesById(id.Value);
            if (User.IsInRole("Owner") || User.IsInRole("Publisher") && page.PublisherID.Contains(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                ViewBag.GroupID = new SelectList(_GroupRepository.GetAllGroups(), "GroupID", "GroupName");
                if (id == null)
                {
                    return NotFound();
                }
                if (page == null)
                {
                    return NotFound();
                }
                return View(page);
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region Edit [HttpPost]
        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PageID,GroupID,Publisher,PublisherID,PageTitle,ShortDescription,Text,ImageFile,ShowInSlider,Tags,ImageName,View,CreateDate")] Page page)
        {
            if (id != page.PageID)
            {
                return NotFound();
            }
            if (User.IsInRole("Owner") || User.IsInRole("Publisher") && page.PublisherID.Contains(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                if (ModelState.IsValid)
                {
                    if (User.IsInRole("Publisher"))
                    {
                        page.Publisher = User.FindFirstValue(ClaimTypes.Name);
                        page.PublisherID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    }
                    if (page.ImageFile != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                        //create folder if not exist
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                        if (page.ImageName != null)
                        {
                            System.IO.File.Delete("wwwroot/uploads/" + page.ImageName);
                        }
                        string fileName = Guid.NewGuid() + page.ImageFile.FileName;

                        string fileNameWithPath = Path.Combine(path, fileName);

                        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                        page.ImageFile.CopyTo(stream);
                        page.ImageName = fileName;
                    }

                    _PageRepository.UpdatePage(page);
                    _PageRepository.save();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Edit));
        }
        #endregion

        #region Delete
        // GET: Admin/Pages/Delete/5
        public IActionResult Delete(int? id)
        {
            Page page = _PageRepository.GetPagesById(id.Value);
            if (User.IsInRole("Owner"))
            {
                if (id == null)
                {
                    return NotFound();
                }
                if (page == null)
                {
                    return NotFound();
                }
                return View(page);
            }
            if (User.IsInRole("Publisher") && page.Publisher.Contains(User.FindFirstValue(ClaimTypes.Name)))
            {
                return View(page);
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region Delete [HttpPost]
        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var page = _PageRepository.GetPagesById(id);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (page.ImageName != null)
            {
                System.IO.File.Delete(Path.Combine(path, page.ImageName));
            }
            _PageRepository.DeletePage(id);
            _PageRepository.save();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _PageRepository.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
