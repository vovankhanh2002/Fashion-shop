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
    public class OderDetailRepon : Repository<OderDetail>, IOderDetail
    {
        private DbcontextBanDo _dbcon;

        public OderDetailRepon(DbcontextBanDo dbcontext):base(dbcontext)
        {
            _dbcon = dbcontext;

        }

        public void updateOderDetail(OderDetail oderDetail)
        {
            _dbcon.Update(oderDetail);
        }
    }
}
