namespace MovieRama.Domain.Services;

using System;
using System.Net;
using System.Threading.Tasks;
using MovieRama.Caching;

/// <summary>
///
/// </summary>
public class CacheService : ICacheService
{
    /// <summary>
    ///
    /// </summary>
    private readonly ICache _cache;

    /// <summary>
    ///
    /// </summary>
    /// <param name="cache"></param>
    public CacheService(ICache cache)
    {
        _cache = cache;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<IResult<T>> GetAsync<T>(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) {
            Result.Error<T>(HttpStatusCode.BadRequest, $"null or empty {nameof(key)}");
        }

        try {
            var res = await _cache.GetAsync<T>(key);

            if (res == null) {
                return Result.Error<T>(HttpStatusCode.NotFound, "key does not exist");
            }

            return Result.Success(res);
        }
        catch (Exception e) {
            return Result.Error<T>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public async Task<IResult<object>> RemoveAsync(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) {
            Result<object>.Error(HttpStatusCode.BadRequest, $"null or empty {nameof(key)}");
        }

        return await _cache.RemoveAsync(key)
            ? Result.Success()
            : Result.Error(HttpStatusCode.NotFound, "");
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="timeToLive"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<IResult<object>> SetAlwaysAsync<T>(string key, T value, TimeSpan timeToLive)
    {
        if (string.IsNullOrWhiteSpace(key)) {
            return Result.Error(HttpStatusCode.BadRequest, $"null or empty {nameof(key)}");
        }

        if (value is null) {
            return Result.Error(HttpStatusCode.BadRequest, $"null {nameof(value)}");
        }

        try {
            var res = await _cache.SetAlwaysAsync(key, value, timeToLive);
            return Result.Success();
        }
        catch (Exception e) {
            return Result.Error(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}