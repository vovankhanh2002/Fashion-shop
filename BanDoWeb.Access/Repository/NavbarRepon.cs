using BanDoWeb.Access.Dbcontext;
using BanDoWeb.Access.Repository.IRepository;
using BanDoWeb.Model.Models;
using Project.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Repository
{
    public class NavbarRepon : Repository<Navbar>, INavbar
    {
        private DbcontextBanDo _dbcon;

        public NavbarRepon(DbcontextBanDo dbcontext):base(dbcontext)
        {
            _dbcon = dbcontext;

        }
        public void UpdateNavar(Navbar navbar)
        {
            navbar.DateSetNavbar = DateTime.Now;
            navbar.DateOutNavbar = DateTime.Now;
            _dbcon.Update(navbar);
            _dbcon.SaveChanges();
        }
    }
}
