namespace MovieRama.WebApp.Controllers;

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Logging;
using MovieRama.Constants;
using MovieRama.Domain;
using MovieRama.Domain.Models;
using MovieRama.Entities;
using MovieRama.WebApp.Models;

public class HomeController : Controller
{
    private readonly IUserService _userService;
    private readonly IMovieService _movieService;
    private readonly SignInManager<Entities.User> _signInManager;
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="movieService"></param>
    /// <param name="signInManager"></param>
    /// <param name="userService"></param>
    public HomeController(ILogger<HomeController> logger,
        IMovieService movieService, SignInManager<User> signInManager,
        IUserService userService)
    {
        _logger = logger;
        _movieService = movieService;
        _signInManager = signInManager;
        _userService = userService;
    }

    public async Task<IActionResult> Index([FromQuery] Guid? userId, [FromQuery] Constants.SortOrder? sortOrder)
    {
        var mresult = await _movieService.ListMoviesAsync(new ListOptions {
            SortOrder = sortOrder ?? Constants.SortOrder.Date,
            SubmitterId = userId
        });

        if (mresult.IsError) {
            return View(new IndexViewModel());
        }

        return View(new IndexViewModel {
            FilterByUserId = userId,
            MovieList = mresult.Data,
            SortOrder = sortOrder ?? Constants.SortOrder.Date
        });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}