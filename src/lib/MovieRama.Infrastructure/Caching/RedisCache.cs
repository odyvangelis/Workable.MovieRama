namespace MovieRama.Infrastructure.Caching;


using System;
using System.Threading.Tasks;

using MovieRama.Caching;
using MovieRama.Extensions;

using StackExchange.Redis;

/// <summary>
/// 
/// </summary>
public class RedisCache : ICache
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IDatabase _db;

    /// <summary>
    ///
    /// </summary>
    /// <param name="db"></param>
    public RedisCache(IDatabase db)
    {
        _db = db;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="ttl"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> SetAlwaysAsync<T>(string key, T value, TimeSpan ttl)
    {
        return await SetAsync(key, value, ttl, When.Always);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="ttl"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> SetIfNotExistsAsync<T>(string key, T value, TimeSpan ttl)
    {
        return await SetAsync(key, value, ttl, When.NotExists);
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetAsync<T>(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        var val = await _db.StringGetAsync(key);

        if (!val.HasValue) {
            return default(T);
        }

        return ((string)val).FromJson<T>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<bool> RemoveAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        return await _db.KeyDeleteAsync(key);
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="ttl"></param>
    /// <param name="when"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private async Task<T> SetAsync<T>(string key, T value, TimeSpan ttl, When when)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (!await _db.StringSetAsync(key, value.ToJson(), ttl, when)) {
            return default(T);
        };

        return value;
    }

}