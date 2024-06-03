namespace MovieRama.Domain;

using System;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface ICacheService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IResult<T>> GetAsync<T>(string key);

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IResult<object>> RemoveAsync(string key);

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeToLive"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    Task<IResult<object>> SetAlwaysAsync<T>(string key, T value,
        TimeSpan timeToLive);
}
