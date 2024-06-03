namespace MovieRama;

using System;
using System.Net;

using MovieRama.Logging;
using MovieRama.Extensions;

/// <summary>
///
/// </summary>
public class Result<T> : IResult<T>
{
    /// <summary>
    ///
    /// </summary>
    public T Data { get; }

    /// <summary>
    ///
    /// </summary>
    public EventId EventId { get; }

    /// <summary>
    ///
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    ///
    /// </summary>
    public HttpStatusCode ErrorCode { get; }

    /// <summary>
    ///
    /// </summary>
    public bool IsSuccess => ErrorCode.IsSuccess();

    /// <summary>
    ///
    /// </summary>
    public bool IsError => !IsSuccess;

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <param name="eventId"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private Result(T data, EventId eventId = EventId.Undefined)
    {
        if (data is null && typeof(T) != typeof(object)) {
            throw new ArgumentNullException(nameof(data));
        }

        Data = data;
        EventId = eventId;
        ErrorCode = HttpStatusCode.OK;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <param name="eventId"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private Result(HttpStatusCode errorCode, string errorMessage,
        EventId eventId = EventId.GenericError)
    {
        if (errorCode.IsSuccess()) {
            throw new ArgumentOutOfRangeException(nameof(errorCode));
        }

        EventId = eventId;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static Result<T> Success(T data, EventId eventId = EventId.Undefined)
    {
        return new Result<T>(data, eventId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public static Result<T> Error(HttpStatusCode errorCode,
        string errorMessage, EventId eventId = EventId.GenericError)
    {
        return new Result<T>(errorCode, errorMessage, eventId);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(T data) => Success(data);

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorResult"></param>
    /// <typeparam name="Y"></typeparam>
    /// <returns></returns>
    public static Result<T> Error<Y>(IResult<Y> errorResult)
    {
        return Error(errorResult.ErrorCode,
            errorResult.ErrorMessage, errorResult.EventId);
    }
}

/// <summary>
///
/// </summary>
public static class Result
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public static IResult<object> Success(EventId eventId = EventId.Undefined) =>
        Result<object>.Success(null, eventId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="data"></param>
    /// <param name="eventId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IResult<T> Success<T>(T data, EventId eventId = EventId.Undefined) =>
        Result<T>.Success(data, eventId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public static IResult<T> Error<T>(HttpStatusCode errorCode, string errorMessage,
        EventId eventId = EventId.GenericError) =>
            Result<T>.Error(errorCode, errorMessage, eventId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <param name="eventId"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IResult<T> BadRequest<T>(string errorMessage,
        EventId eventId = EventId.GenericError) =>
            Result<T>.Error(HttpStatusCode.BadRequest, errorMessage, eventId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorResult"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IResult<T> Error<T>(IResult errorResult) => Error<T>(
        errorResult.ErrorCode, errorResult.ErrorMessage, errorResult.EventId);

    /// <summary>
    ///
    /// </summary>
    /// <param name="errorCode"></param>
    /// <param name="errorMessage"></param>
    /// <param name="eventId"></param>
    /// <returns></returns>
    public static IResult<object> Error(HttpStatusCode errorCode,
        string errorMessage, EventId eventId = EventId.GenericError) =>
            Result<object>.Error(errorCode, errorMessage, eventId);
}
