namespace MovieRama.Domain.Models
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class SubmitMovieOptions
    {
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
    }
}
