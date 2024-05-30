namespace MovieRama.Infrastructure.Data;

using MovieRama.Config;

using Microsoft.EntityFrameworkCore;

/// <summary>
///
/// </summary>
public sealed class AppDbContext : DbContext
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    /// <summary>
    ///
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
