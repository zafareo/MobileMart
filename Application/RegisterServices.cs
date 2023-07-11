
using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class RegisterServices
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MapProfiles));
            return services;
        }
    }
}
