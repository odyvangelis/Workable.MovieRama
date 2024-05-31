namespace MovieRama.Domain.Models;

/// <summary>
///
/// </summary>
public class CreateUserOptions
{
    /// <summary>
    ///
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string ConfirmPassword { get; set; }
}
