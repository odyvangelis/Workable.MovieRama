using Microsoft.AspNetCore.Mvc;

namespace MovieRama.WebApp.Controllers;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using MovieRama.Domain;
using MovieRama.Entities;
using MovieRama.Domain.Models;
using MovieRama.WebApp.Models;

[Authorize]
public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    private readonly UserManager<Entities.User> _userManager;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="movieService"></param>
    /// <param name="userManager"></param>
    public MovieController(IMovieService movieService, UserManager<User> userManager)
    {
        _movieService = movieService;
        _userManager = userManager;
    }
    
    [HttpGet]
    public IActionResult Submit()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] Guid? userId, [FromQuery] Constants.SortOrder? sortOrder)
    {
        var mresult = await _movieService.ListMoviesAsync(new ListOptions {
            SubmitterId = userId,
            SortOrder = sortOrder ?? Constants.SortOrder.Date
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

    [HttpPost]
    public async Task<IActionResult> Vote(Guid id, [FromQuery] Constants.VoteType type,
        [FromQuery] Guid? filteredUser, [FromQuery] Constants.SortOrder? appliedOrder)
    {
        var redirectUrl = "~/Movie/Index/?";

        if (filteredUser is not null) {
            redirectUrl += $"userId={filteredUser.Value}&";
        }

        if (appliedOrder is not null) {
            redirectUrl += $"sortOrder={appliedOrder.Value}";
        }
        
        var userIdraw = _userManager.GetUserId(User);
        if (!Guid.TryParse(userIdraw, out var userId)) {
            return Redirect(redirectUrl);
        }

        await _movieService.VoteAsync(
            new VoteOptions {
                MovieId = id,
                VoteType = type,
                UserId = userId
            });
        
        return Redirect(redirectUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit([Bind("Title, Description")] SubmitMovieViewModel model)
    {
        if (!ModelState.IsValid) {
            return View(model);
        }

        var userIdraw = _userManager.GetUserId(User);
        if (!Guid.TryParse(userIdraw, out var userId)) {
            return View(model);
        }

        var result = await _movieService.SubmitMovieAsync(
            new SubmitMovieOptions {
                Title = model.Title,
                SubmitterId = userId,
                Description = model.Description,
            });

        if (result.IsSuccess) {
            return Redirect($"~/Movie/Index");
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}