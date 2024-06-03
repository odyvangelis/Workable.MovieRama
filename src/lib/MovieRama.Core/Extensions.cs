namespace MovieRama.Extensions;

using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class Extensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool IsSuccess(this HttpStatusCode code)
    {
        return (int)code is (>= 200 and < 300);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="s"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string Truncate(this string s, int length)
    {
        if (string.IsNullOrEmpty(s)) {
            return s;
        }

        if (s.Length <= length) {
            return s;
        }

        return s[..length];
    }
    
    /// <summary>
    ///
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="prettyPrint"></param>
    /// <param name="camelCase"></param>
    /// <returns></returns>
    public static string ToJson(this object obj,
        bool prettyPrint = true, bool camelCase = false)
    {
        ArgumentNullException.ThrowIfNull(obj);

        var options = new JsonSerializerOptions {
            WriteIndented = prettyPrint,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        if (camelCase) {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        return JsonSerializer.Serialize(obj, options);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="str"></param>
    /// <param name="camelCase"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FromJson<T>(this string str, bool camelCase = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);

        var options = new JsonSerializerOptions {
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        if (camelCase) {
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        return JsonSerializer.Deserialize<T>(str, options);
    }
}
