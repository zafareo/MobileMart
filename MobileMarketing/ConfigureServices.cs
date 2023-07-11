using Application.Interfaces;
using MobileMarketing.Services;

namespace MobileMarketing
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddWebUIServices(this IServiceCollection services)
        {            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
