using AsisyaApi.Application.Interfaces;
using AsisyaApi.Infrastructure.Background;
using AsisyaApi.Infrastructure.Persistence;
using AsisyaApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AsisyaApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<IBackgroundJobQueue, BackgroundJobQueue>();
        services.AddSingleton<IProductBulkJobStore, ProductBulkJobStore>();
        services.AddHostedService<QueuedHostedService>();
        return services;
    }
}
