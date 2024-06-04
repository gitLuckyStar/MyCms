using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;

namespace MyCms.Components
{
    public class ShowGroupsComponents : ViewComponent
    {
        private IGroupRepository _GroupRepository;
        public ShowGroupsComponents(IGroupRepository GroupRepository)
        {
            _GroupRepository = GroupRepository;
        }
        public IViewComponentResult Invoke()
        {
            return View("/Views/Shared/ViewComponents/_ShowGroupsInLayout.cshtml", _GroupRepository.GetGroupsForView());
        }
    }
}
