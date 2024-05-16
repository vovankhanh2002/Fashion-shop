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
    public class ContactRepon : Repository<Contact>, IContact
    {
        private DbcontextBanDo _dbcon;

        public ContactRepon(DbcontextBanDo dbcontext):base(dbcontext)
        {
            _dbcon = dbcontext;

        }

        public void UpdateContact(Contact contact)
        {
            _dbcon.Update(contact);
            _dbcon.SaveChanges();
        }

        
    }
}
