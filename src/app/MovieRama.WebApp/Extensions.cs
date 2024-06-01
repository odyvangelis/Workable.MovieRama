namespace MovieRama.WebApp;

using System;
using System.Linq;

public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="movieId"></param>
    /// <returns></returns>
    public static bool Likes(this Entities.User user, Guid movieId)
    {
        if (user is null) {
            return false;
        }

        return user.Liked.Any(x => x.Id == movieId);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="movieId"></param>
    /// <returns></returns>
    public static bool Hates(this Entities.User user, Guid movieId)
    {
        if (user is null) {
            return false;
        }

        return user.Hated.Any(x => x.Id == movieId);
    }
    
}