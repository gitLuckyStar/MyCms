using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class PageCommentRepository : IPageCommentRepository
    {
        private readonly MyCmsContext db;
        public PageCommentRepository(MyCmsContext db)
        {
            this.db = db;
        }

        public bool AddComment(PageComment comment)
        {
            
            try
            {
                db.pageComments.Add(comment);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public IEnumerable<PageComment> GetAllPageComments()
        {
            return db.pageComments;
        }

        public IEnumerable<PageComment> GetCommentsByNewsID(int pageid)
        {
            return db.pageComments.Where(c => c.PageID == pageid);
        }
    }
}
