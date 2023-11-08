namespace Kira.Controllers;

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

[Route("Culture/[action]")] public class CultureController : Controller
{
    public IActionResult SetCulture(string? culture, string redirectUri)
    {
        if (culture is not null)
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new(culture)));

        return LocalRedirect(redirectUri);
    }
}