namespace MovieRama.Tests.Services;

using MovieRama.Domain;
using MovieRama.Tests.Fixtures;

/// <summary>
/// 
/// </summary>
public class CacheService_Tests : MovieRamaTestBase
{
    private readonly ICacheService _cache;
    
    public CacheService_Tests(ServiceResolver services)
    {
        _cache = services.Resolve<ICacheService>();
    }

    [Fact]
    public async Task SetAlwaysAsync_Should_Set_Key_And_Value()
    {
        var key = $"{Guid.NewGuid():N}";
        var value = $"{Guid.NewGuid():N}";

        var result = await _cache.SetAlwaysAsync(key, value, TimeSpan.FromMinutes(10));
        
        Assert.True(result.IsSuccess);

        var gresult = await _cache.GetAsync<string>(key);
        Assert.True(gresult.IsSuccess);
        Assert.Equal(value, gresult.Data);
    }
    
    [Fact]
    public async Task RemoveAsync_Should_Remove_Key()
    {
        var key = $"{Guid.NewGuid():N}";
        var value = $"{Guid.NewGuid():N}";

        var sresult = await _cache.SetAlwaysAsync(key, value, TimeSpan.FromMinutes(10));
        
        Assert.True(sresult.IsSuccess);

        var rresult = await _cache.RemoveAsync(key);
        Assert.True(rresult.IsSuccess);
        
        var gresult = await _cache.GetAsync<string>(key);
        Assert.False(gresult.IsSuccess);
    }
}