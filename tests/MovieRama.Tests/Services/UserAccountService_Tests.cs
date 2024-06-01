namespace MovieRama.Tests.Services;

using MovieRama.Domain;
using MovieRama.Tests.Fixtures;

public class UserAccountService_Tests : MovieRamaTestBase
{
    private readonly IUserService _userService;
    
    public UserAccountService_Tests(ServiceResolver resolver)
    {
        _userService = resolver.Resolve<IUserService>();
    }

    [Fact]
    public async Task CreateUserAsync_Should_Create_New_User()
    {
        var opts = new Domain.Models.CreateUserOptions {
            FullName = "Test User",
            Email = $"{Guid.NewGuid():N}@localhost",
            Password = "Pass123$",
            ConfirmPassword = "Pass123$"
        };

        var result = await _userService.CreateUserAsync(opts);

        Assert.True(result.IsSuccess,
            $"{result.ErrorCode:D} - {result.ErrorMessage} ");

        Assert.NotNull(result.Data);

        Assert.Equal(opts.Email, result.Data.Email);
        Assert.Equal(opts.FullName, result.Data.FullName);
    }
}