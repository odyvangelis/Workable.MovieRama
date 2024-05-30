namespace MovieRama.Config;

using Microsoft.Extensions.Configuration;

/// <summary>
///
/// </summary>
public class ConnectionConfig
{
    /// <summary>
    ///
    /// </summary>
    [ConfigurationKeyName("redis")]
    public string Redis { get; set; }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationKeyName("postgres")]
    public string Postgres { get; set; }

    /// <summary>
    ///
    /// </summary>
    [ConfigurationKeyName("movierama_db")]
    public string Database { get; set; }
}
