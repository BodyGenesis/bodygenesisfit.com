using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MediatR;
using Piranha;
using Piranha.AttributeBuilder;
using Piranha.Data.EF.PostgreSql;
using Piranha.Manager.Editor;
using Piranha.Services;

using BodyGenesis.Core;
using BodyGenesis.Presentation.Website.Cms.Blocks;
using Microsoft.Extensions.Options;
using Piranha.Data.EF.SQLite;
using BodyGenesis.Core.UseCases;

namespace BodyGenesis.Presentation.Website
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly WebsiteOptions _websiteOptions;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _websiteOptions = configuration.GetSection("BodyGenesis:Presentation:Website").Get<WebsiteOptions>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPiranha(options =>
            {
                options.AddRazorRuntimeCompilation = _webHostEnvironment.IsDevelopment();

                options.UseFileStorage();
                options.UseImageSharp();
                options.UseManager();
                options.UseTinyMCE();
                options.UseMemoryCache();

                if (_webHostEnvironment.IsDevelopment())
                {
                    options.UseEF<SQLiteDb>(dbOptions =>
                    {
                        dbOptions.UseSqlite(_websiteOptions.ConnectionString);
                    });
                }

                else
                {
                    options.UseEF<PostgreSqlDb>(dbOptions =>
                    {
                        dbOptions.UseNpgsql(_websiteOptions.ConnectionString);
                    });

                    services.Configure<ForwardedHeadersOptions>(options =>
                    {
                        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                        options.KnownNetworks.Clear();
                        options.KnownProxies.Clear();
                    });
                }
            });

            services.AddAuth0(_configuration);
            services.AddAuthorization(options =>
            {
                options.AddPolicy("BackofficePolicy", policy =>
                {
                    policy.RequireClaim(Piranha.Manager.Permission.Admin);
                });

                options.AddPolicy("MembershipPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApi api, IMediator mediator)
        {
            App.Init(api);
            App.CacheLevel = Piranha.Cache.CacheLevel.Basic;

            App.Blocks.Register<LoggedInBlock>();
            App.Blocks.Register<LoggedOutBlock>();

            new ContentTypeBuilder(api)
                .AddAssembly(typeof(Startup).Assembly)
                .Build()
                .DeleteOrphans();

            EditorConfig.FromFile("editorconfig.json");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseForwardedHeaders();
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UsePiranha(options =>
            {
                options.UseManager();
                options.UseTinyMCE();
            });

            mediator.Send(new SeedMembershipPlansRequest()).Wait();
        }
    }
}
