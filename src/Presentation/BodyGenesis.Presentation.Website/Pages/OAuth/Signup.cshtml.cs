using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BodyGenesis.Presentation.Website.Pages.OAuth
{
    public class SignupModel : PageModel
    {
        public async Task OnGetAsync()
        {
            var redirectUri = HttpContext.Request.Query["returnUrl"];

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            }

            var authProperties = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };

            authProperties.SetParameter("login_hint", "signup");

            await HttpContext.ChallengeAsync("Auth0", authProperties);
        }
    }
}
