namespace MovieRama.Domain.Services;

using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using MovieRama.Core;
using MovieRama.Data;
using MovieRama.Logging;
using MovieRama.Entities;

/// <summary>
///
/// </summary>
public class UserService : IUserService
{
    /// <summary>
    ///
    /// </summary>
    private readonly IRepository _repo;

    /// <summary>
    ///
    /// </summary>
    private readonly UserManager<Entities.User> _userManager;


    /// <summary>
    ///
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="repo"></param>
    public UserService(UserManager<User> userManager, IRepository repo)
    {
        _userManager = userManager;
        _repo = repo;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<IResult<Entities.User>> GetUserByIdAsync(Guid userId)
    {
        var user = await _repo.GetQueryable<Entities.User>()
            .Include(x => x.Liked)
            .Include(x => x.Hated)
            .Include(x => x.Submitted)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user is null) {
            return Result.Error<Entities.User>(
                HttpStatusCode.NotFound, $"user does not exist");
        }

        return Result.Success(user);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<IResult<Entities.User>> CreateUserAsync(Models.CreateUserOptions options)
    {
        if (options is null) {
            return Result.Error<Entities.User>(HttpStatusCode.BadRequest,
                $"null {nameof(options)}", EventId.UserServiceCreateUserValidationError);
        }

        if (string.IsNullOrWhiteSpace(options.Email)) {
            return Result.Error<Entities.User>(HttpStatusCode.BadRequest,
                $"null {nameof(options.Email)}", EventId.UserServiceCreateUserValidationError);
        }

        if (string.IsNullOrWhiteSpace(options.FullName)) {
            return Result.Error<Entities.User>(HttpStatusCode.BadRequest,
                $"null {nameof(options.FullName)}", EventId.UserServiceCreateUserValidationError);
        }

        if (string.IsNullOrWhiteSpace(options.Password)) {
            return Result.Error<Entities.User>(HttpStatusCode.BadRequest,
                $"null {nameof(options.Password)}", EventId.UserServiceCreateUserValidationError);
        }

        if (!options.Password.Equals(options.ConfirmPassword)) {
            return Result.Error<Entities.User>(HttpStatusCode.BadRequest,
                $"passwords do not match", EventId.UserServiceCreateUserValidationError);
        }

        var user = new Entities.User {
            Email = options.Email,
            UserName = options.Email,
            FullName = options.FullName.Truncate(Constants.Validation.MaxNameLength)
        };

        var result = await _userManager.CreateAsync(user, options.Password);
        if (!result.Succeeded) {
            return Result.Error<Entities.User>(HttpStatusCode.BadGateway,
                string.Join(", ", result.Errors.Select(x => $"{x.Code} - {x.Description}")), EventId.UserServiceCreateUserFailed);
        }

        return Result.Success(user);
    }
}
