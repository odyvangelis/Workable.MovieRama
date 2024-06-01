namespace MovieRama.Domain.Services;

using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using MovieRama.Core;
using MovieRama.Data;
using MovieRama.Logging;

/// <summary>
///
/// </summary>
public class MovieService : IMovieService
{
    /// <summary>
    ///
    /// </summary>
    private readonly IRepository _repo;

    /// <summary>
    ///
    /// </summary>
    private readonly IUserService _users;

    /// <summary>
    ///
    /// </summary>
    /// <param name="users"></param>
    /// <param name="repo"></param>
    public MovieService(IUserService users, IRepository repo)
    {
        _users = users;
        _repo = repo;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<IResult<Entities.Movie>> SubmitMovieAsync(Models.SubmitMovieOptions options)
    {
        if (options is null) {
            return Result.Error<Entities.Movie>(HttpStatusCode.BadRequest,
                $"null {nameof(options)}", EventId.MovieServiceSubmitMovieValidationError);
        }

        if (string.IsNullOrWhiteSpace(options.Title)) {
            return Result.Error<Entities.Movie>(HttpStatusCode.BadRequest,
                $"null {nameof(options.Title)}", EventId.MovieServiceSubmitMovieValidationError);
        }

        if (string.IsNullOrWhiteSpace(options.Description)) {
            return Result.Error<Entities.Movie>(HttpStatusCode.BadRequest,
                $"null {nameof(options.Description)}", EventId.MovieServiceSubmitMovieValidationError);
        }

        var uresult = await _users.GetUserByIdAsync(options.SubmitterId);
        if (uresult.IsError) {
            return Result.Error<Entities.Movie>(uresult);
        }

        var movie = new Entities.Movie {
            Submitter = uresult.Data,
            Title = options.Title.Truncate(Constants.Validation
                .MaxMovieTitle), //tbd - could also return validation error
            Description = options.Description.Truncate(Constants.Validation.MaxMovieDesc)
        };

        _repo.Add(movie);
        var cresult = await _repo.TryCommitAsync();
        
        return cresult.IsError
            ? Result.Error<Entities.Movie>(HttpStatusCode.InternalServerError,
                "failed to submit movie", EventId.MovieServiceSubmitMovieFailed)
            : Result.Success(movie);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="movieId"></param>
    /// <returns></returns>
    public async Task<IResult<Dto.MovieInfo>> GetMovieInfoAsync(Guid movieId)
    {
        var movie = await _repo.GetQueryable<Entities.Movie>()
            .Where(x => x.Id == movieId)
            .Select(x => new Dto.MovieInfo {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                SubmitterId = x.SubmitterId,
                LikeCount = x.LikedBy.Count,
                HateCount = x.HatedBy.Count,
                SubmitterName = x.Submitter.FullName,
                DateSubmitted = x.AuditInfo.CreatedUtc
            })
            .FirstOrDefaultAsync();

        if (movie is null) {
            return Result.Error<Dto.MovieInfo>(HttpStatusCode.NotFound, "movie does not exist");
        }

        return Result.Success(movie);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<IResult<List<Dto.MovieInfo>>> ListMoviesAsync(Models.ListOptions options)
    {
        var query  = _repo.GetQueryable<Entities.Movie>();

        if (options.SubmitterId is not null) {
            query = query.Where(x => x.SubmitterId == options.SubmitterId.Value);
        }

        switch (options.SortOrder) {
            case Constants.SortOrder.Date:
                query = query.OrderByDescending(x => x.AuditInfo.CreatedUtc)
                    .ThenByDescending(x => x.LikedBy.Count);
                break;
            case Constants.SortOrder.Like:
                query = query.OrderByDescending(x => x.LikedBy.Count)
                    .ThenByDescending(x => x.AuditInfo.CreatedUtc);
                break;
            case Constants.SortOrder.Hate:
                query = query.OrderByDescending(x => x.HatedBy.Count)
                    .ThenByDescending(x => x.AuditInfo.CreatedUtc);
                break;
        }

        var result = await query
            .Select(x => new Dto.MovieInfo {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                SubmitterId = x.SubmitterId,
                LikeCount = x.LikedBy.Count,
                HateCount = x.HatedBy.Count,
                SubmitterName = x.Submitter.FullName,
                DateSubmitted = x.AuditInfo.CreatedUtc
            })
            .ToListAsync();

        return Result.Success(result);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public async Task<IResult<object>> VoteAsync(Models.VoteOptions options)
    {
        if (options is null) {
            return Result.Error(HttpStatusCode.BadRequest,
                $"null {nameof(options)}", EventId.MovieServiceVoteValidationError);
        }

        var movie = await _repo.GetQueryable<Entities.Movie>()
            .Include(x => x.LikedBy)
            .Include(x => x.HatedBy)
            .FirstOrDefaultAsync(x => x.Id == options.MovieId);

        if (movie is null) {
            return Result.Error(HttpStatusCode.NotFound, $"movie {options.MovieId} does not exist",
                EventId.MovieServiceVoteValidationError);
        }

        if (movie.SubmitterId == options.UserId) {
            return Result.Error(HttpStatusCode.Conflict, $"users cannot vote on their own submissions",
                EventId.MovieServiceVoteValidationError);
        }

        var uresult = await _users.GetUserByIdAsync(options.UserId);
        if (uresult.IsError) {
            return Result.Error<object>(uresult);
        }

        switch (options.VoteType) {
            case Constants.VoteType.Remove:
                movie.HatedBy.Remove(uresult.Data);
                movie.LikedBy.Remove(uresult.Data);
                break;
            case Constants.VoteType.Like:
                movie.LikedBy.Add(uresult.Data);
                movie.HatedBy.Remove(uresult.Data);
                break;
            case Constants.VoteType.Hate:
                movie.HatedBy.Add(uresult.Data);
                movie.LikedBy.Remove(uresult.Data);
                break;
        }

        movie.AuditInfo.Update();
        var cresult = await _repo.TryCommitAsync();

        return cresult.IsError
            ? Result.Error(HttpStatusCode.InternalServerError,
                "failed to submit vote", EventId.MovieServiceSubmitVoteFailed)
            : Result.Success();
    }
}
