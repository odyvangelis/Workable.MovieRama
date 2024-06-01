namespace MovieRama.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MovieRama.Domain.Config;
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
    public static IServiceCollection AddMovieRama(this IServiceCollection services, IConfiguration config)
    {
        var appConfig = config.Get<MovieRama.Config.ApplicationConfig>();
        return services
            .AddSingleton(appConfig)
            .AddDomain()
            .AddInfrastructure(appConfig);
    }
}
