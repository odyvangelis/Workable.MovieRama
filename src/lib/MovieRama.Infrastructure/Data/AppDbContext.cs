namespace MovieRama.Infrastructure.Data;

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using MovieRama.Entities;

/// <summary>
///
/// </summary>
public sealed class AppDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
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

        modelBuilder.Entity<User>()
            .Property(x => x.FullName)
            .HasMaxLength(50);
        
        modelBuilder.Entity<User>()
            .HasMany(x => x.Submitted)
            .WithOne(x => x.Submitter)
            .HasForeignKey(x => x.SubmitterId);

        modelBuilder.Entity<User>()
            .HasMany(x => x.Liked)
            .WithMany(x => x.LikedBy)
            .UsingEntity(j => j.ToTable("UserLikes"));

        modelBuilder.Entity<User>()
            .HasMany(x => x.Hated)
            .WithMany(x => x.HatedBy)
            .UsingEntity(j => j.ToTable("UserHates"));

        modelBuilder.Entity<Movie>()
            .Property(x => x.Description)
            .HasMaxLength(600);
        modelBuilder.Entity<Movie>()
            .Property(x => x.Title)
            .HasMaxLength(100);

        modelBuilder.Entity<Movie>().ComplexProperty(x => x.AuditInfo, p =>
        {
            p.Property(a => a.CreatedUtc).HasColumnName("CreatedUtc");
            p.Property(a => a.UpdatedUtc).HasColumnName("UpdatedUtc");
            p.IsRequired();
        });
    }

}
