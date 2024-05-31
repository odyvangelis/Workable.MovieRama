namespace MovieRama.Core;

using System.Net;

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
}
