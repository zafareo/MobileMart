using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.IdentityEntites;
using Domain.Models.Token;
using Domain.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction
{
    public interface IApplicationDbContext
    {
        public DbSet<Phone> Phones { get; }
        public DbSet<Order> Orders { get; }
        public DbSet<SubCategory> Subcategories { get; }
        public DbSet<OrderPhone> OrdersPhones { get; }
        public DbSet<Announcement> Announcements { get; }
        public DbSet<User> Users { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<UserRole> UserRoles { get; }
        public DbSet<Permission> Permissions { get; }
        public DbSet<RolePermission> RolePermissions { get; }
        public DbSet<UserRefreshToken> UserRefreshToken { get; }
        DbSet<T> Set<T>() where T : class; 
        Task<int> SaveChangesAsync(CancellationToken token = default);
    }
}
