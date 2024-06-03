namespace MovieRama.Domain;

using System.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public interface IMovieService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IResult<object>> VoteAsync(Models.VoteOptions options);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IResult<List<Dto.MovieInfo>>> ListMoviesAsync(Models.ListOptions options);

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    Task<IResult<Entities.Movie>> SubmitMovieAsync(Models.SubmitMovieOptions options);
}