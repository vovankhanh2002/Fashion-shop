using BanDoWeb.Model.Models;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Repository.IRepository
{
    public interface IOderHeader : IRepository<OderHeader>
    {
        void updateOderHeader(OderHeader oderHeader);
        void updateStatus(int id, string status, string? paymentStatus= null);
    }
}
