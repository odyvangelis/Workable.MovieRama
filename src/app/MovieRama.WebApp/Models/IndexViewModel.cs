namespace MovieRama.WebApp.Models;

using System;
using System.Collections.Generic;
using MovieRama.Domain.Dto;

/// <summary>
/// 
/// </summary>
public class IndexViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public Guid? FilterByUserId { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public Constants.SortOrder SortOrder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<Domain.Dto.MovieInfo> MovieList { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IndexViewModel()
    {
        MovieList = new List<MovieInfo>();
    }
}