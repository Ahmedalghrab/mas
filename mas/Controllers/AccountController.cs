using mas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mas.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpPost("LoginPost")]
    public async Task<IActionResult> Login([FromForm] LoginRequest request)
    {
        var returnUrl = string.IsNullOrWhiteSpace(request.ReturnUrl) ? "/admin" : request.ReturnUrl;
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = "/admin";

        var result = await _signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            request.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
            return LocalRedirect(returnUrl);

        var error = result.IsLockedOut
            ? "Account locked. Please try again later."
            : result.IsNotAllowed
                ? "Login not allowed. Please confirm your email."
                : "Invalid email or password.";

        var loginUrl = Url.Content("~/Account/Login");
        return Redirect($"{loginUrl}?ReturnUrl={Uri.EscapeDataString(returnUrl)}&error={Uri.EscapeDataString(error)}");
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
