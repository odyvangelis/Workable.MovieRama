namespace MovieRama.Entities;

using System;
using System.Collections.Generic;
using MovieRama.Entities.Complex;

/// <summary>
/// 
/// </summary>
public class Movie
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Guid SubmitterId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Complex.AuditInfo AuditInfo { get; set; }

    #region Navigation
    
    /// <summary>
    /// 
    /// </summary>
    public virtual User Submitter { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<User> LikedBy { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<User> HatedBy { get; set; }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public Movie()
    {
        Id = Guid.NewGuid();
        LikedBy = new List<User>();
        HatedBy = new List<User>();
        AuditInfo = new AuditInfo();
    }
}