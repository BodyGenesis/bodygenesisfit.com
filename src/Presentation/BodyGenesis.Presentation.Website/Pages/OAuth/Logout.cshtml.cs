using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BodyGenesis.Presentation.Website.Pages.OAuth
{
    public class LogoutModel : PageModel
    {
        public async Task OnGetAsync()
        {
            await HttpContext.SignOutAsync("Auth0", new AuthenticationProperties { RedirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
