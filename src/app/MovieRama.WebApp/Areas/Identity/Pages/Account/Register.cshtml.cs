namespace MovieRama.WebApp.Areas.Identity.Pages.Account;

using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;


using MovieRama.Entities;

/// <summary>
///
/// </summary>
public class RegisterModel : PageModel
{
    /// <summary>
    ///
    /// </summary>
    private readonly Domain.IUserService _userService;

    /// <summary>
    ///
    /// </summary>
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    ///
    /// </summary>
    /// <param name="signInManager"></param>
    /// <param name="userService"></param>
    public RegisterModel(
        SignInManager<User> signInManager,
        Domain.IUserService userService)
    {
        _signInManager = signInManager;
        _userService = userService;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
        await Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (!ModelState.IsValid) {
            return Page();
        }
        var result = await _userService.CreateUserAsync(new Domain.Models.CreateUserOptions {
            Email = Input.Email,
            FullName = Input.FullName,
            Password = Input.Password,
            ConfirmPassword = Input.ConfirmPassword
        });

        if (result.IsError) {
            ModelState.AddModelError(string.Empty, result.ErrorMessage);
            return Page();
        }

        await _signInManager.SignInAsync(result.Data, isPersistent: false);
        return LocalRedirect(returnUrl);
    }
}
