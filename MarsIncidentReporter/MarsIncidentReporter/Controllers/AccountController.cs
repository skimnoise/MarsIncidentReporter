using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        // Redirect to Auth0 login page
        var redirectUrl = Url.Action(nameof(Callback), "Account");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, "Auth0");
    }

    public async Task<IActionResult> Callback()
    {
        // Authenticate the user and get the result
        var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Check if authentication was successful
        if (!authenticateResult.Succeeded)
        {
            return RedirectToAction("Error", "Home");
        }

        var userRoles = authenticateResult.Principal?.Claims
            .Where(c => c.Type == "https://localhost:7181/roles")
            .SelectMany(c => c.Value.Split(','))
            .ToList();

        // Redirect based on roles
        if (userRoles.Contains("Admin"))
        {
            return RedirectToAction("AdminOnly", "Home");
        }
        if (userRoles.Contains("Reader"))
        {
            return RedirectToAction("ReaderOnly", "Home");
        }

        // Default redirect if no specific role matches
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme, "Auth0");
    }
}
