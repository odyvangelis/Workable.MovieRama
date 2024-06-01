namespace MovieRama.Domain.Dto;

using System;

/// <summary>
/// 
/// </summary>
public class MovieInfo
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
    public int HateCount { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int LikeCount { get; set; }

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
    public string SubmitterName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DateTime DateSubmitted { get; set; }
}