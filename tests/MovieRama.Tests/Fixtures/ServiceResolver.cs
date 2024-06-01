namespace MovieRama.Tests.Fixtures;

using System;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
///
/// </summary>
public class ServiceResolver : IDisposable
{
    private readonly IServiceScope _scope;

    public ServiceResolver(InitializationFixture fixture)
    {
        _scope = fixture.CreateScope();
    }

    public T Resolve<T>()
    {
        return _scope.ServiceProvider.GetRequiredService<T>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
    }
}
