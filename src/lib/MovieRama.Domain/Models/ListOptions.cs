namespace MovieRama.Domain.Models;

using System;

/// <summary>
/// 
/// </summary>
public class ListOptions
{
    /// <summary>
    /// 
    /// </summary>
    public Guid? SubmitterId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Constants.SortOrder SortOrder { get; set; }
}