using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IPageCommentRepository
    {
        IEnumerable<PageComment> GetAllPageComments();
        IEnumerable<PageComment> GetCommentsByNewsID(int pageid);
        bool AddComment(PageComment comment);
    }
}
