using Microsoft.Extensions.DependencyInjection;
using PrjThalesTest.Business.Interfaces;
using PrjThalesTest.Business.Services;

namespace PrjThalesTest.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services)
        {
            services.AddScoped<IProductoService, ProductoService>();

            return services;
        }
    }
}
