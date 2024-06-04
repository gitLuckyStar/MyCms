using DataLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCms.Components
{    
    public class SliderComponents :ViewComponent
    {
        private readonly IPageRepository _PageRepository;

        public SliderComponents(IPageRepository pageRepository)
        {
            _PageRepository = pageRepository;
        }
        public IViewComponentResult Invoke()
        {
            return View("/Views/Shared/ViewComponents/_Slider.cshtml", _PageRepository.Slider(true));
        }
    }
}
