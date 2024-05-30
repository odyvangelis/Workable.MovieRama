namespace MovieRama.Configuration;

using Microsoft.Extensions.DependencyInjection;

using MovieRama.Infrastructure.Config;

/// <summary>
/// 
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IServiceCollection AddMovieRama(this IServiceCollection services, Config.ApplicationConfig config)
    {
        services.AddInfrastructure(config);
        return services;
    }
}