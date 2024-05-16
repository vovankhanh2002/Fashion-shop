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
    public class OderHeaderRepon : Repository<OderHeader>, IOderHeader
    {
        private DbcontextBanDo _dbcon;

        public OderHeaderRepon(DbcontextBanDo dbcontext):base(dbcontext)
        {
            _dbcon = dbcontext;

        }

        public void updateOderHeader(OderHeader oderHeader)
        {
            _dbcon.Update(oderHeader);
        }

        public void updateStatus(int id, string status, string? paymentStatus = null)
        {
            var oderHeader = _dbcon.OderHeaders.FirstOrDefault(i => i.Id == id);
            if(oderHeader != null)
            {
                oderHeader.OderStatus = status;
                if(paymentStatus != null)
                {
                    oderHeader.PaymentStatus = paymentStatus;
                }
            }
        }
    }
}
