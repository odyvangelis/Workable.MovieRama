namespace MovieRama;

using System.Net;
using System.Text.Json.Serialization;

/// <summary>
///
/// </summary>
public interface IResult<out T> : IResult
{
    /// <summary>
    ///
    /// </summary>
    T Data { get; }
}

public interface IResult
{
    /// <summary>
    ///
    /// </summary>
    [JsonIgnore]
    bool IsError { get; }

    /// <summary>
    ///
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    ///
    /// </summary>
    string ErrorMessage { get; }

    /// <summary>
    ///
    /// </summary>
    Logging.EventId EventId { get; }

    /// <summary>
    ///
    /// </summary>
    HttpStatusCode ErrorCode { get; }
}
