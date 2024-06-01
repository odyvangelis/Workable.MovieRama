namespace MovieRama.WebApp.Models;

using System.ComponentModel.DataAnnotations;

public class SubmitMovieViewModel
{
    [Required]
    [StringLength(Constants.Validation.MaxMovieTitle)]
    public string Title { get; set; }
    
    [Required]
    [StringLength(Constants.Validation.MaxMovieDesc)]
    public string Description { get; set; }
}