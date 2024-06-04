using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace MyCms.Controllers
{    
    public class NewsController : Controller
    {
        private IGroupRepository _GroupRepository;
        private IPageRepository _PageRepository;
        private IPageCommentRepository _PageCommentRepository;
        private readonly SignInManager<IdentityUser> _signInManager;

        public NewsController(IGroupRepository GroupRepository,
                                IPageRepository PageRepository,
                                IPageCommentRepository PageCommentRepository,
                                SignInManager<IdentityUser> signInManager)
        {
            _GroupRepository = GroupRepository;
            _PageRepository = PageRepository;
            _PageCommentRepository = PageCommentRepository;
            _signInManager = signInManager;
        }
        #region Index
        public IActionResult Index()
        {
            return View(_PageRepository.AllPages());
        }
        #endregion

        #region GroupNews
        [Route("Group/{id}/{title}")]
        public IActionResult GroupNews(int id,string title)
        {
            ViewBag.Name = title;
            return View(_PageRepository.ShowGroupsByid(id));
        }
        #endregion

        #region ShowNews
        public IActionResult ShowNews(int? id, [Bind("View")]Page page)
        {
            if (id == null)
            {
                return NotFound();
            }

            page = _PageRepository.GetPagesById(id.Value);
            if (page == null)
            {
                return NotFound();
            }
            if(_signInManager.IsSignedIn(User))
            {
                if (!System.IO.File.ReadAllText("wwwroot/logs/UserPageVisits.txt").Contains(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString() + "/" + page.PageID.ToString()))
                {
                    System.IO.File.AppendAllLines("wwwroot/logs/UserPageVisits.txt", new[] { User.FindFirstValue(ClaimTypes.NameIdentifier).ToString() + "/" + page.PageID.ToString() });
                    page.View +=1;
                    _PageRepository.UpdatePage(page);
                    _PageRepository.save();
                }
            }            
            return View(page);
        }
        #endregion

        #region AddComment
        public IActionResult AddComment(int id,string comment)
        {
          
            PageComment addcomment = new PageComment()
            {
                CreateDate = DateTime.Now,
                PageID = id,
                CommentText = comment,
                Email = User.FindFirstValue(ClaimTypes.Email),
                Name = User.FindFirstValue(ClaimTypes.Name)
            };
            _PageCommentRepository.AddComment(addcomment);
            return ViewComponent("CommentListComponents",id);
        }
        #endregion
    }
}
