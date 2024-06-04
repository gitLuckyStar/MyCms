using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer
{
    public class MyCmsContext:DbContext

    {

        public MyCmsContext(DbContextOptions<MyCmsContext> options) : base(options)
        {

        }
        public DbSet<Group> Groups { get; set; }

        public DbSet<Page> pages { get; set; }

        public DbSet<PageComment> pageComments { get; set; }
    }
}
