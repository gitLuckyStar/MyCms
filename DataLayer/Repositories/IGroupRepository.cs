using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IGroupRepository:IDisposable
    {
        IEnumerable<Group> GetAllGroups();
        Group GetGroupById(int groupid);
        bool InsertGroup(Group Group);
        bool UpdateGroup(Group Group);
        bool DeleteGroup(Group Group);
        bool DeleteGroup(int groupid);
        void save();
        IEnumerable<ShowGroupsViewModel> GetGroupsForView();
    }
}
