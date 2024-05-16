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
    public class NumberOfVisitsRepon : Repository<NumberOfVisits>, INumberOfVisits
    {
        private DbcontextBanDo _dbcon;

        public NumberOfVisitsRepon(DbcontextBanDo dbcontext) : base(dbcontext)
        {
            _dbcon = dbcontext;

        }

        public void UpdateNumberOfVisits(NumberOfVisits numberOfVisits)
        {
            _dbcon.Update(numberOfVisits);
        }
    }
}
