namespace MovieRama.Infrastructure.Config;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRama.Caching;
using MovieRama.Data;
using MovieRama.Config;
using MovieRama.Entities;
using MovieRama.Infrastructure.Caching;
using MovieRama.Infrastructure.Data;
using StackExchange.Redis;

/// <summary>
/// 
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        ApplicationConfig configuration)
    {
        services.AddDbContext<AppDbContext>(opts => {
            opts.UseLazyLoadingProxies();
            opts.UseNpgsql(configuration.ConnectionStrings.Database);
        });
        services.AddScoped<IRepository, AppRepository>();
        services.AddDefaultIdentity<User>(options => {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>();

        var connectionString = configuration.ConnectionStrings.Redis;

        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(connectionString));

        services.AddScoped<ICache>(x => {
            var database = x.GetRequiredService<IConnectionMultiplexer>()
                .GetDatabase();

            return new RedisCache(database);
        });
        
        return services;
    }
}