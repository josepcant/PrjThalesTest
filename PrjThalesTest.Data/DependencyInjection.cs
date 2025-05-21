using Microsoft.Extensions.DependencyInjection;
using PrjThalesTest.Data.Interfaces;
using PrjThalesTest.Data.Repositories;

namespace PrjThalesTest.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {
            services.AddHttpClient<IProductoRepository, ProductoRepository>(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            return services;
        }
    }
}
