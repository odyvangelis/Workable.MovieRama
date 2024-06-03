namespace MovieRama.Caching;

using System;
using System.Threading.Tasks;

/// <summary>
///
/// </summary>
public interface ICache
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(string key);

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="ttl"></param>
    /// <param name="when"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> SetAlwaysAsync<T>(string key, T value, TimeSpan ttl);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="ttl"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<T> SetIfNotExistsAsync<T>(string key, T value, TimeSpan ttl);
}