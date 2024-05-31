namespace MovieRama.Domain;

using System;
using System.Threading.Tasks;

/// <summary>
///
/// </summary>
public interface IUserService
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IResult<Entities.User>> GetUserByIdAsync(Guid userId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IResult<Entities.User>> CreateUserAsync(Models.CreateUserOptions options);
}
