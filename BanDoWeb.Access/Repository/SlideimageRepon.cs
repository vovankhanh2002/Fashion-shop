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
    public class SlideimageRepon : Repository<SlidedImage>, ISlideimage
    {
        private DbcontextBanDo _dbcon;

        public SlideimageRepon(DbcontextBanDo dbcontext):base(dbcontext)
        {
            _dbcon = dbcontext;

        }
        public void UpdateSlidedImage(SlidedImage slidedImage)
        {
            _dbcon.Update(slidedImage);
            _dbcon.SaveChanges();
        }
    }
}
