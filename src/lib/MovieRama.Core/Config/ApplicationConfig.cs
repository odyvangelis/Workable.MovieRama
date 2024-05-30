namespace MovieRama.Config;

using Microsoft.Extensions.Configuration;

/// <summary>
///
/// </summary>
public class ApplicationConfig
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationKeyName("ApplicationName")]
    public string ApplicationName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationKeyName("ConnectionStrings")]
    public ConnectionConfig ConnectionStrings { get; set; }
}
