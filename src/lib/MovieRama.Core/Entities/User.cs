namespace MovieRama.Entities;

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// 
/// </summary>
public class User : IdentityUser<Guid>
{
    /// <summary>
    /// 
    /// </summary>
    public string FullName { get; set; }

    #region Navigation

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<Movie> Liked { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<Movie> Hated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<Movie> Submitted { get; set; }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public User()
    {
        Id = Guid.NewGuid();
        Liked = new List<Movie>();
        Hated = new List<Movie>();
        Submitted = new List<Movie>();
    }
}