﻿using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext) 
        {

        }
    }
}
