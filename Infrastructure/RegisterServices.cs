using Application.Abstraction;
using Application.Interfaces;
using Infrastructure.DataAccess;
using Infrastructure.DataAccess.Interceptor;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<IApplicationDbContext, MobileMarketingDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DbConnection")));
            services.AddScoped<AuditableSaveChangesInterceptor>();
            services.AddTransient<IJWTService, JWTServices>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<IUserRefreshTokenRepository, UserRefreshTokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhoneRepository, PhoneRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderPhoneRepository, OrderPhoneRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncmentRepository>();
            return services;

        }
    }
}
