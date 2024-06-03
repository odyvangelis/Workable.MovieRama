namespace MovieRama.Domain.Config;

using Microsoft.Extensions.DependencyInjection;

using MovieRama.Domain.Services;

/// <summary>
///
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<ICacheService, CacheService>();
        return services;
    }
}
