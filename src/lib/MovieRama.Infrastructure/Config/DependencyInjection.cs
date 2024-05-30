namespace MovieRama.Infrastructure.Config;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MovieRama.Data;
using MovieRama.Config;
using MovieRama.Infrastructure.Data;


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
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseLazyLoadingProxies();
            opts.UseNpgsql(configuration.ConnectionStrings.Database);
        });
        services.AddScoped<IRepository, AppRepository>();

        return services;
    }
    //
    // /// <summary>
    // ///
    // /// </summary>
    // /// <param name="services"></param>
    // /// <returns></returns>
    // public static IServiceCollection AddAspNetIdentity(this IServiceCollection services)
    // {
    //     services.AddIdentity<MegaventoryUser, IdentityRole>(options =>
    //         {
    //             options.User.RequireUniqueEmail = false;
    //             options.User.AllowedUserNameCharacters =
    //                 "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-:";
    //         })
    //         .AddEntityFrameworkStores<>()
    //         .AddDefaultTokenProviders()
    //         .AddUserManager<Identity.MegaventoryUserManager<Model.Admin.MegaventoryUser>>();
    //
    //     return services;
    // } 
}