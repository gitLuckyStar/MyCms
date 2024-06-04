using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IPageRepository:IDisposable
    {
        IEnumerable<Page> GetAllPages();
        Page GetPagesById(int PageID);
        bool InsertPage(Page page);
        bool UpdatePage(Page page);
        bool DeletePage(Page page);
        bool DeletePage(int PageID);
        void save();

        IEnumerable<Page> TopNews(int take = 5);
        IEnumerable<Page> ShowGroupsByid(int groupid);
        IEnumerable<Page> LastNews(int take = 5);
        IEnumerable<Page> SearchPages(string parameter);
        IEnumerable<Page> AllPages();
        IEnumerable<Page> GetPublisherPage(string publisher);
        IEnumerable<Page> Slider(bool slider);
    }
}
