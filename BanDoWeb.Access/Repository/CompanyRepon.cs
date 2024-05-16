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
    public class CompanyRepon : Repository<Company>, ICompany
    {
        private DbcontextBanDo _dbcon;

        public CompanyRepon(DbcontextBanDo dbcontext) : base(dbcontext)
        {
            _dbcon = dbcontext;

        }
        public void UpdateCampany(Company company)
        {
            _dbcon.Update(company);
        }
    }
}
