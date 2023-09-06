using Application.TradeItems;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceConfigureExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddDepencyInjection();

            return services;
        }

        private static IServiceCollection AddDepencyInjection(this IServiceCollection services)
        {
            services
                .AddScoped<ITradeItemService, TradeItemService>();

            return services;
        }
    }
}
