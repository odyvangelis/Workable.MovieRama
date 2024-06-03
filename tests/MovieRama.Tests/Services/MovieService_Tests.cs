namespace MovieRama.Tests.Services;

using System.Net;
using Microsoft.EntityFrameworkCore;

using MovieRama.Data;
using MovieRama.Domain;
using MovieRama.Tests.Fixtures;

public class MovieService_Tests : MovieRamaTestBase, IAsyncLifetime
{
    private readonly IRepository _repo;
    private readonly ICacheService _cache;
    private readonly IUserService _userService;
    private readonly IMovieService _movieService;

    private Entities.User _testUser;

    private List<Entities.User> _generatedUsers = [];
    private List<Entities.Movie> _generatedMovies = [];

    public MovieService_Tests(ServiceResolver resolver)
    {
        _repo = resolver.Resolve<IRepository>();
        _cache = resolver.Resolve<ICacheService>();
        _userService = resolver.Resolve<IUserService>();
        _movieService = resolver.Resolve<IMovieService>();
    }

    public async Task InitializeAsync()
    {
        _testUser = await CreateTestUserAsync();

        var movies = await _repo.GetQueryable<Entities.Movie>()
            .ToListAsync();

        _repo.DeleteRange(movies);

        await _repo.CommitAsync();

        await _cache.RemoveAsync(Caching.Key.ListMovieInfo);
        
        var opts = Enumerable.Range(0, 10)
            .Select(x =>
                new Domain.Models.SubmitMovieOptions {
                    SubmitterId = _testUser.Id,
                    Title = $"Test-Movie-{x}",
                    Description = $"Test Description {x}"
                })
            .ToList();

        foreach (var option in opts) {
            var result = await _movieService.SubmitMovieAsync(option);
            Assert.True(result.IsSuccess);
            _generatedMovies.Add(result.Data);
        }

        var rand = new Random();
        foreach (var i in Enumerable.Range(0, 10)) {
            var testUser = await CreateTestUserAsync();
            foreach (var movie in _generatedMovies) {
                var vresult = await _movieService.VoteAsync(
                    new Domain.Models.VoteOptions {
                        MovieId = movie.Id,
                        VoteType = rand.Next(100) > 66
                            ? Constants.VoteType.Like
                            : Constants.VoteType.Hate,
                        UserId = testUser.Id
                    });
                
                Assert.True(vresult.IsSuccess);
            }
        }
    }

    public async Task DisposeAsync()
    {
        //will cascade delete movies and votes
        foreach (var usr in _generatedUsers) {
            await _userService.DeleteUserAsync(usr.Id);
        }

        await _cache.RemoveAsync(Caching.Key.ListMovieInfo);
    }

    [Fact]
    public async Task SubmitMovieAsync_Should_Submit_New_Movie()
    {
        var opts = new Domain.Models.SubmitMovieOptions {
            SubmitterId = _testUser.Id,
            Title = "Star Wars Episode V - The Empire Strikes Back",
            Description = """
                          After the Rebel Alliance are overpowered by the Empire,
                          Luke Skywalker begins his Jedi training with Yoda,
                          while his friends are pursued across the galaxy by
                          Darth Vader and bounty hunter Boba Fett.
                          """
        };

        var result = await _movieService.SubmitMovieAsync(opts);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(opts.Title, result.Data.Title);
        Assert.Equal(opts.Description, result.Data.Description);
        Assert.Equal(opts.SubmitterId, result.Data.SubmitterId);
        Assert.Empty(result.Data.LikedBy);
        Assert.Empty(result.Data.HatedBy);

        Assert.Single(_testUser.Submitted.Where(x => x.Id == result.Data.Id));
    }
    
    [Fact]
    public async Task SubmitMovieAsync_With_No_Title_Should_Fail()
    {
        var opts = new Domain.Models.SubmitMovieOptions {
            SubmitterId = _testUser.Id,
            Description = """
                          After the Rebel Alliance are overpowered by the Empire,
                          Luke Skywalker begins his Jedi training with Yoda,
                          while his friends are pursued across the galaxy by
                          Darth Vader and bounty hunter Boba Fett.
                          """
        };

        var result = await _movieService.SubmitMovieAsync(opts);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(HttpStatusCode.BadRequest, result.ErrorCode);
        Assert.Equal(Logging.EventId.MovieServiceSubmitMovieValidationError, result.EventId);
    }

    [Fact]
    public async Task SubmitMovieAsync_With_No_Description_Should_Fail()
    {
        var opts = new Domain.Models.SubmitMovieOptions {
            SubmitterId = _testUser.Id,
            Title = "Lord Of The Rings - The Fellowship Of The Ring"
        };

        var result = await _movieService.SubmitMovieAsync(opts);
        
        Assert.False(result.IsSuccess);
        Assert.Equal(HttpStatusCode.BadRequest, result.ErrorCode);
        Assert.Equal(Logging.EventId.MovieServiceSubmitMovieValidationError, result.EventId);
    }

    [Theory]
    [InlineData(Constants.SortOrder.Date)]
    [InlineData(Constants.SortOrder.Like)]
    [InlineData(Constants.SortOrder.Hate)]
    public async Task ListMoviesAsync_Should_Return_MovieInfo_In_Correct_Order(Constants.SortOrder sortOrder)
    {
        // arrange
        _generatedMovies = sortOrder switch {
            Constants.SortOrder.Like => _generatedMovies
                .OrderByDescending(x => x.LikedBy.Count)
                .ThenByDescending(x => x.AuditInfo.CreatedUtc)
                .ToList(),

            Constants.SortOrder.Hate => _generatedMovies
                .OrderByDescending(x => x.HatedBy.Count)
                .ThenByDescending(x => x.AuditInfo.CreatedUtc)
                .ToList(),

            Constants.SortOrder.Date => _generatedMovies
                .OrderByDescending(x => x.AuditInfo.CreatedUtc)
                .ThenByDescending(x => x.LikedBy.Count)
                .ToList(),

            _ => _generatedMovies
        };

        // act
        var lresult = await _movieService.ListMoviesAsync(
            new Domain.Models.ListOptions {
                SortOrder = sortOrder
            });
        
        // assert
        Assert.True(lresult.IsSuccess);
        Assert.NotEmpty(lresult.Data);
        Assert.Equal(_generatedMovies.Count, lresult.Data.Count);

        for (var i = 0; i < _generatedMovies.Count; i++) {
            Assert.Equal(_generatedMovies[i].Id, lresult.Data[i].Id);
        }
    }

    [Fact]
    public async Task VoteAsync_Should_Change_Counts()
    {
        //arrange
        var testMovie = _generatedMovies[0];
        var testUser = await CreateTestUserAsync();

        var originalLikes = testMovie.LikedBy.Count;
        var originalHates = testMovie.HatedBy.Count;

        // act
        var vresult = await _movieService.VoteAsync(
            new Domain.Models.VoteOptions {
                UserId = testUser.Id,
                MovieId = testMovie.Id,
                VoteType =  Constants.VoteType.Like
            });

        // assert
        Assert.True(vresult.IsSuccess);
        Assert.Equal(originalLikes + 1, testMovie.LikedBy.Count);

        // act
        vresult = await _movieService.VoteAsync(
            new Domain.Models.VoteOptions {
                UserId = testUser.Id,
                MovieId = testMovie.Id,
                VoteType =  Constants.VoteType.Hate
            });

        // assert
        Assert.True(vresult.IsSuccess);
        Assert.Equal(originalLikes, testMovie.LikedBy.Count);
        Assert.Equal(originalHates + 1, testMovie.HatedBy.Count);
        
        // act
        vresult = await _movieService.VoteAsync(
            new Domain.Models.VoteOptions {
                UserId = testUser.Id,
                MovieId = testMovie.Id,
                VoteType =  Constants.VoteType.Remove
            });

        // assert
        Assert.True(vresult.IsSuccess);
        Assert.Equal(originalLikes, testMovie.LikedBy.Count);
        Assert.Equal(originalHates, testMovie.HatedBy.Count);
    }
    
    [Fact]
    public async Task VoteAsync_On_Own_Movie_Should_Fail()
    {
        var testMovie = _generatedMovies[0];

        var vresult = await _movieService.VoteAsync(
            new Domain.Models.VoteOptions {
                MovieId = testMovie.Id,
                UserId = testMovie.SubmitterId,
                VoteType =  Constants.VoteType.Like
            });

        Assert.False(vresult.IsSuccess);
        Assert.Equal(HttpStatusCode.Conflict, vresult.ErrorCode);
        Assert.Equal(Logging.EventId.MovieServiceVoteValidationError, vresult.EventId);
    }

    private async Task<Entities.User> CreateTestUserAsync()
    {
        var opts = new Domain.Models.CreateUserOptions {
            FullName = "Test User",
            Email = $"{Guid.NewGuid():N}@localhost",
            Password = "Pass123$",
            ConfirmPassword = "Pass123$"
        };

        var result = await _userService.CreateUserAsync(opts);
        Assert.True(result.IsSuccess);
        _generatedUsers.Add(result.Data);

        return result.Data;
    }
}