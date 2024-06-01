namespace MovieRama.Tests.Fixtures;
using System;
using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieRama.Configuration;

public class InitializationFixture : IDisposable
{
    private readonly IServiceScope _scope;

    public InitializationFixture()
    {
        var serviceCollection = CreateDefaultServiceCollection();

        var provider = serviceCollection.BuildServiceProvider();

        _scope = provider.CreateScope();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    public IServiceScope CreateScope()
    {
        return _scope.ServiceProvider.CreateScope();
    }
    
    public static IServiceCollection CreateDefaultServiceCollection()
    {
        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("./Config/appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var serviceCollection = new ServiceCollection()
            .Configure<IConfiguration>(config)
            .AddSingleton(config)
            .AddMovieRama(config);

        return serviceCollection;
    }
}