using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BodyGenesis.AccountManager.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MudBlazor;
using MudBlazor.Services;

namespace BodyGenesis.AccountManager.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddMudServices(options =>
            {
                options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;

                options.SnackbarConfiguration.PreventDuplicates = true;
                options.SnackbarConfiguration.NewestOnTop = false;
                options.SnackbarConfiguration.ShowCloseIcon = true;
                options.SnackbarConfiguration.VisibleStateDuration = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
                options.SnackbarConfiguration.HideTransitionDuration = 500;
                options.SnackbarConfiguration.ShowTransitionDuration = 500;
                options.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
            });

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Auth0", options.ProviderOptions);
            });

            builder.Services.AddHttpClient<CustomersApiClient>(hc => hc.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            await builder.Build().RunAsync();
        }
    }
}
