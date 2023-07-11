using Application.Abstraction;
using Domain.Models;
using Domain.Models.Entities;
using Domain.Models.IdentityEntites;
using Domain.Models.Token;
using Domain.Models.UserModels;
using Infrastructure.DataAccess.Interceptor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess
{
    public class MobileMarketingDbContext : DbContext, IApplicationDbContext
    {
         private readonly AuditableSaveChangesInterceptor _interceptor;
        public MobileMarketingDbContext( DbContextOptions<MobileMarketingDbContext> options, AuditableSaveChangesInterceptor interceptor)
            : base(options)
        {
            _interceptor = interceptor;
        }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }

        public DbSet<UserRefreshToken> UserRefreshToken { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SubCategory> Subcategories { get; set; }

        public DbSet<OrderPhone> OrdersPhones { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_interceptor);
        }
    }
}
