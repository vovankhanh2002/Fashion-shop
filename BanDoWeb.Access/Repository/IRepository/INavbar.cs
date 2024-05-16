﻿using BanDoWeb.Model.Models;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Repository.IRepository
{
    public interface INavbar : IRepository<Navbar>
    {
        void UpdateNavar(Navbar navbar);
    }
}
