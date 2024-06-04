using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class PageRepository : IPageRepository
    {
        private readonly MyCmsContext db;
        public PageRepository(MyCmsContext db)
        {
            this.db = db;
        }
        public IEnumerable<Page> GetAllPages()
        {
            return db.pages;
        }

        public Page GetPagesById(int PageID)
        {
            return db.pages.Find(PageID);
        }

        public bool InsertPage(Page page)
        {
            try
            {
                db.pages.Add(page);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdatePage(Page page)
        {
            try
            {
                db.Entry(page).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool DeletePage(Page page)
        {
            try
            {
                db.Entry(page).State = EntityState.Deleted;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool DeletePage(int PageID)
        {
            try
            {
                var page = GetPagesById(PageID);
                DeletePage(page);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public void save()
        {
            db.SaveChanges();
        }
        public void Dispose()
        {
            db.Dispose();
        }

        public IEnumerable<Page> TopNews(int take =5)
        {
            return db.pages.OrderByDescending(p =>p.View).Take(take);
        }

        public IEnumerable<Page> ShowGroupsByid(int groupid)
        {
            return db.pages.Where(p => p.GroupID == groupid);
                }

        public IEnumerable<Page> LastNews(int take = 5)
        {
            return db.pages.OrderByDescending(p => p.CreateDate).Take(take);
        }

        public IEnumerable<Page> SearchPages(string parameter)
        {
            return db.pages.Where(p => p.PageTitle.Contains(parameter) || p.ShortDescription.Contains(parameter) ||
            p.Tags.Contains(parameter) || p.Text.Contains(parameter)).Distinct();
        }

        public IEnumerable<Page> AllPages()
        {
            return db.pages.OrderByDescending(p =>p.CreateDate);
        }

        public IEnumerable<Page> GetPublisherPage(string publisher)
        {
            return db.pages.Where(p => p.PublisherID.Contains(publisher));
        }

        public IEnumerable<Page> Slider(bool slider)
        {
            return db.pages.Where(p => p.ShowInSlider == true);
        }
    }
}
