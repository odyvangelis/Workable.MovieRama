namespace MovieRama.Infrastructure.Data;

using MovieRama.Infrastructure.Data;

/// <summary>
///
/// </summary>
public sealed class AppRepository : EfRepository
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="context"></param>
    public AppRepository(AppDbContext context) : base(context)
    { }
}
