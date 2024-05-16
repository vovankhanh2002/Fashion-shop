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
    public class ProductRepon : Repository<Product>, IProduct
    {
        private DbcontextBanDo _dbcon;
        public ProductRepon(DbcontextBanDo dbcontext) : base(dbcontext)
        {
            _dbcon = dbcontext;
        }
        public void updateProduct(Product product)
        {
            _dbcon.Update(product);
        }
    }
}
