using Core.Interfaces;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceConfigureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddDepencyInjection();

            return services;
        }

        private static IServiceCollection AddDepencyInjection(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}
