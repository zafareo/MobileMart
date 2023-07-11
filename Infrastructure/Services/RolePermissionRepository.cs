using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.IdentityEntites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IApplicationDbContext context)
            : base(context)
        {
        }
    }
}
