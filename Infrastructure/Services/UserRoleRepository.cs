using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IApplicationDbContext context)
            : base(context)
        {

        }
    }
}
