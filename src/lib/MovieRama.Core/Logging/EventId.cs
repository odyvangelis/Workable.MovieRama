namespace MovieRama.Logging;

/// <summary>
///
/// </summary>
public enum EventId
{
    /// <summary>
    ///
    /// </summary>
    Undefined = 0,

    /// <summary>
    ///
    /// </summary>
    GenericError = 1,

    /// <summary>
    ///
    /// </summary>
    GenericInformation = 3,

    /// <summary>
    ///
    /// </summary>
    InternalServerError = 500,
    
    /// <summary>
    /// 
    /// </summary>
    RepositoryTryCommitFailed = 600,

    /* UserService EventIds  */

    /// <summary>
    ///
    /// </summary>
    UserServiceCreateUserValidationError = 1000,

    /// <summary>
    ///
    /// </summary>
    UserServiceCreateUserFailed = 1001,

    /* MovieService EventIds */

    /// <summary>
    ///
    /// </summary>
    MovieServiceSubmitMovieValidationError = 2000,

    /// <summary>
    /// 
    /// </summary>
    MovieServiceSubmitMovieFailed = 2001,

    /// <summary>
    ///
    /// </summary>
    MovieServiceVoteValidationError = 2010,
    
    /// <summary>
    /// 
    /// </summary>
    MovieServiceSubmitVoteFailed = 2011,
}
