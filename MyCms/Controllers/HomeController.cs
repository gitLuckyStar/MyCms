using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyCms.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using Microsoft.EntityFrameworkCore;

namespace MyCms.Controllers
{
    public class HomeController : Controller
    {
        private IPageRepository _PageRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
                              IPageRepository PageRepository)
        {
            _PageRepository = PageRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_PageRepository.LastNews());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {            
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
