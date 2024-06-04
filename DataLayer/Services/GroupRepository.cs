using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer
{
    public class GroupRepository : IGroupRepository
    {
        private readonly MyCmsContext db;
        public GroupRepository(MyCmsContext db)
        {
            this.db = db;
        }
        public IEnumerable<Group> GetAllGroups()
        {
            return db.Groups;
        }

        public Group GetGroupById(int groupid)
        {
            return db.Groups.Find(groupid);
        }

        public bool InsertGroup(Group pageGroup)
        {
            try
            {
                db.Groups.Add(pageGroup);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool UpdateGroup(Group pageGroup)
        {
            try
            {
                db.Entry(pageGroup).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public bool DeleteGroup(Group pageGroup)
        {
            try
            {
                db.Entry(pageGroup).State = EntityState.Deleted;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteGroup(int groupid)
        {
            try
            {
                var group = GetGroupById(groupid);
                DeleteGroup(group);
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

        public IEnumerable<ShowGroupsViewModel> GetGroupsForView()
        {
            return db.Groups.Select(G => new ShowGroupsViewModel()
            {
                GroupID = G.GroupID,
                GroupName = G.GroupName,
                PageCount = G.Pages.Count,
            });
        }

    }
}
