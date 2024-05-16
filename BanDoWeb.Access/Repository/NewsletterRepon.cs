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
    public class NewsletterRepon : Repository<Newsletter>, INewsletter
    {
        private DbcontextBanDo _dbcon;

        public NewsletterRepon(DbcontextBanDo dbcontext):base(dbcontext)
        {
            _dbcon = dbcontext;

        }
        public void UpdateNewsletter(Newsletter newsletter)
        {
            _dbcon.Update(newsletter);
            _dbcon.SaveChanges();
        }
    }
}
