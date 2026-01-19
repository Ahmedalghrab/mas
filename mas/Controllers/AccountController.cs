using mas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace mas.Controllers;

[Route("Account")]
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
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
        {
            // Check user roles for debugging
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation($"User {user.Email} logged in with roles: {string.Join(", ", roles)}");
                
                // If trying to access admin and not in Admin role, redirect to home
                if (returnUrl.StartsWith("/admin", StringComparison.OrdinalIgnoreCase) && !roles.Contains("Admin"))
                {
                    return Redirect("/?message=لا تملك صلاحية الوصول لهذه الصفحة");
                }
            }
            
            return LocalRedirect(returnUrl);
        }

        var error = result.IsLockedOut
            ? "الحساب مقفل. حاول مرة أخرى لاحقاً."
            : result.IsNotAllowed
                ? "تسجيل الدخول غير مسموح. يرجى تأكيد بريدك الإلكتروني."
                : "البريد الإلكتروني أو كلمة المرور غير صحيحة.";

        var loginUrl = Url.Content("~/Account/Login");
        return Redirect($"{loginUrl}?ReturnUrl={Uri.EscapeDataString(returnUrl)}&error={Uri.EscapeDataString(error)}");
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return Redirect("/");
    }

    [HttpPost("CustomerLoginPost")]
    public async Task<IActionResult> CustomerLogin([FromForm] LoginRequest request)
    {
        var returnUrl = string.IsNullOrWhiteSpace(request.ReturnUrl) ? "/" : request.ReturnUrl;
        if (!Url.IsLocalUrl(returnUrl))
            returnUrl = "/";

        var result = await _signInManager.PasswordSignInAsync(
            request.Email,
            request.Password,
            request.RememberMe,
            lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation($"Customer {user.Email} logged in with roles: {string.Join(", ", roles)}");
                
                // إذا كان المستخدم أدمن، نقله إلى لوحة التحكم
                if (roles.Contains("Admin"))
                {
                    return Redirect("/admin");
                }
            }
            
            return LocalRedirect(returnUrl);
        }

        var error = result.IsLockedOut
            ? "تم قفل حسابك مؤقتاً. يرجى المحاولة لاحقاً."
            : result.IsNotAllowed
                ? "تسجيل الدخول غير مسموح. يرجى تأكيد بريدك الإلكتروني."
                : "البريد الإلكتروني أو كلمة المرور غير صحيحة.";

        return Redirect($"/customer/login?ReturnUrl={Uri.EscapeDataString(returnUrl)}&error={Uri.EscapeDataString(error)}");
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
