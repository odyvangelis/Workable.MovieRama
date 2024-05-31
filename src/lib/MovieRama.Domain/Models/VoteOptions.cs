namespace MovieRama.Domain.Models
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class VoteOptions
    {
        /// <summary>
        ///
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Guid MovieId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Constants.VoteType VoteType { get; set; }
    }
}
