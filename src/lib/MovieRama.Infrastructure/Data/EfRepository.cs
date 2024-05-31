namespace MovieRama.Infrastructure.Data;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net;
using Microsoft.EntityFrameworkCore;

using MovieRama.Data;
using MovieRama.Logging;

/// <summary>
///
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class EfRepository : IRepository
{
    private readonly DbContext context_;

    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public EfRepository(DbContext context)
    {
        context_ = context;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    public void Add<T>(T entity) where T : class
    {
        if (IsAttached(entity)) {
            return;
        }
        context_.Set<T>().Add(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    public void AddRange<T>(System.Collections.Generic.IEnumerable<T> range) where T : class
    {
        context_.Set<T>().AddRange(range.Where(x => !IsAttached(x)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    public void Update<T>(T entity) where T : class
    {
        context_.Entry(entity).State = EntityState.Modified;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    public void Delete<T>(T entity) where T : class
    {
        context_.Set<T>().Remove(entity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    public void DeleteRange<T>(IEnumerable<T> range) where T : class
    {
        context_.Set<T>().RemoveRange(range);
    }

    /// <summary>
    /// xxx(ov) todo - trycatch
    /// </summary>
    /// <returns></returns>
    public async Task<int> CommitAsync()
    {
        return await context_.SaveChangesAsync();
    }

    public async Task<IResult<object>> TryCommitAsync()
    {
        try {
            await context_.SaveChangesAsync();
            return Result.Success();
        }
        catch (Exception e) {
            return Result.Error(
                HttpStatusCode.InternalServerError, e.Message, EventId.RepositoryTryCommitFailed);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public int Commit()
    {
        return context_.SaveChanges();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IQueryable<T> ScalarSqlQuery<T>(string sql, params object[] parameters)
    {
        return context_.Database.SqlQueryRaw<T>(sql, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public int RawSqlQuery(string sql, params object[] parameters)
    {
        return context_.Database.ExecuteSqlRaw(sql, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public async Task<int> RawSqlQueryAsync(string sql, params object[] parameters)
    {
        return await context_.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IQueryable<T> GetQueryable<T>() where T : class
    {
        return context_.Set<T>().AsQueryable();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetById<T>(params object[] id) where T : class
    {
        return context_.Set<T>().Find(id);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T> GetByIdAsync<T>(params object[] id) where T : class
    {
        return await context_.Set<T>().FindAsync(id);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private bool IsAttached<T>(T entity) where T : class
    {
        var entry = context_.Entry(entity);
        return entry is not null && entry.State != EntityState.Detached;
    }
}
