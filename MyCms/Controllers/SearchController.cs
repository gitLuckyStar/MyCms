using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;

namespace MyCms.Controllers
{
    public class SearchController : Controller
    {
        private IPageRepository _PageRepository;

        public SearchController(IPageRepository PageRepository)
        {
            _PageRepository = PageRepository;
        }

        public IActionResult Index(string q)
        {
            ViewBag.Name = q;
            return View(_PageRepository.SearchPages(q));
        }
    }
}
