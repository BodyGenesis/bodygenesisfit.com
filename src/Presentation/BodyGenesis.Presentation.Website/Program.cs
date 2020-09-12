using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using BodyGenesis.Core;

namespace BodyGenesis.Presentation.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddOptions()
                        .Configure<WebsiteOptions>(context.Configuration.GetSection("BodyGenesis:Presentation:Website"))
                        .AddMediatR(typeof(BodyGenesisCoreMarkerType))
                        .AddBodyGenesis(context.Configuration, builder =>
                        {
                            builder.UseMongoDB();
                            builder.UseSendGrid();
                        });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
