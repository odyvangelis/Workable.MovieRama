namespace MovieRama.Data;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
public interface IRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    void Add<T>(T entity) where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    void AddRange<T>(IEnumerable<T> range) where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    void Update<T>(T entity) where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <param name="entity"></param>
    /// <typeparam name="T"></typeparam>
    void Delete<T>(T entity) where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <param name="range"></param>
    /// <typeparam name="T"></typeparam>
    void DeleteRange<T>(IEnumerable<T> range) where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    int Commit();

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Task<int> CommitAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IResult<object>> TryCommitAsync();

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    int RawSqlQuery(string sql, params object[] parameters);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<int> RawSqlQueryAsync(string sql, params object[] parameters);

    /// <summary>
    ///
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="parameters"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IQueryable<T> ScalarSqlQuery<T>(string sql, params object[] parameters);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IQueryable<T> GetQueryable<T>() where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    T GetById<T>(params object[] id) where T : class;

    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="Tid"></typeparam>
    /// <returns></returns>
    Task<T> GetByIdAsync<T>(params object[] id)
        where T : class;
}