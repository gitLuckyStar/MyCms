using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;

namespace MyCms.Components
{
    public class TopNewsComponents:ViewComponent
    {
        private IPageRepository _PageRepository;
        public TopNewsComponents (IPageRepository PageRepository)
        {
            _PageRepository = PageRepository;
        }
        public IViewComponentResult Invoke()
        {
            return View("/Views/Shared/ViewComponents/_TopNews.cshtml", _PageRepository.TopNews());
        }
    }
}
