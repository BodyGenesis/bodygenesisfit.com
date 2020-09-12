using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using BodyGenesis.Core.UseCases;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class IServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth0(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/oauth/login");
                    options.AccessDeniedPath = new PathString("/oauth/login");
                    options.LogoutPath = new PathString("/oauth/logout");
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                })
                .AddOpenIdConnect("Auth0", options =>
                {
                    options.Authority = $"https://{configuration["Auth0:Domain"]}";
                    options.ClientId = configuration["Auth0:ClientId"];
                    options.ClientSecret = configuration["Auth0:ClientSecret"];
                    options.ResponseType = OpenIdConnectResponseType.Code;

                    options.Scope.Add("openid profile email");

                    options.CallbackPath = new PathString("/oauth/callback");
                    options.ClaimsIssuer = "Auth0";

                    options.TokenValidationParameters.NameClaimType = "name";

                    options.Events = new OpenIdConnectEvents
                    {
                        OnRedirectToIdentityProviderForSignOut = (context) =>
                        {
                            var logoutUri = $"https://{configuration["Auth0:Domain"]}/v2/logout?client_id={configuration["Auth0:ClientId"]}";

                            var postLogoutUri = context.Properties.RedirectUri;

                            if (!string.IsNullOrEmpty(postLogoutUri))
                            {
                                if (postLogoutUri.StartsWith("/"))
                                {
                                    var request = context.Request;

                                    postLogoutUri = $"https://{request.Host}{request.PathBase}{postLogoutUri}";
                                }

                                logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                            }

                            context.Response.Redirect(logoutUri);
                            context.HandleResponse();

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = async (context) =>
                        {
                            if (context.Principal.Claims.FirstOrDefault(c => c.Type.Equals("email") && c.Value.EndsWith("@bodygenesisfit.com", StringComparison.OrdinalIgnoreCase)) != null)
                            {
                                context.Principal.AddIdentity(new ClaimsIdentity(Piranha.Manager.Permission.All().Select(p => new Claim(p, p))));
                            }

                            var sub = context.Principal.FindFirstValue("sub");
                            var name = context.Principal.FindFirstValue("name");
                            var email = context.Principal.FindFirstValue("email");

                            await context.HttpContext.RequestServices
                                .GetRequiredService<IMediator>()
                                .Send(new EnsureCustomerForAuth0UserRequest(sub, name, email));
                        }
                    };

                    var jwtHandler = new JwtSecurityTokenHandler();

                    jwtHandler.InboundClaimTypeMap.Clear();

                    options.SecurityTokenValidator = jwtHandler;

                    options.ClaimActions.Clear();
                    options.ClaimActions.MapAll();
                }).Services;
        }
    }
}
