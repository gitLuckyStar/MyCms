using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;

namespace MyCms.Components
{
    
    public class CommentListComponents:ViewComponent
    {
        private IPageCommentRepository _PageCommentRepository;
        public CommentListComponents(IPageCommentRepository PageCommentRepository)
        {
            _PageCommentRepository = PageCommentRepository;
        }
        public IViewComponentResult Invoke(int id)
        {            
            return View("/Views/Shared/ViewComponents/_CommentList.cshtml", _PageCommentRepository.GetCommentsByNewsID(id));
        }
    }
}
