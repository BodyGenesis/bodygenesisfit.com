using BodyGenesis.Core.Services;
using BodyGenesis.Infrastructure.SendGrid;
using Microsoft.Extensions.Options;
using SendGrid;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class BodyGenesisAppBuilderExtensions
    {
        public static BodyGenesisAppBuilder UseSendGrid(this BodyGenesisAppBuilder builder, Action<SendGridOptions> configure = null)
        {
            configure = configure ?? new Action<SendGridOptions>(o => { });

            builder.AddOptions<SendGridOptions>()
                .Bind(builder.Configuration.GetSection("BodyGenesis:Infrastructure:SendGrid"))
                .Configure(configure);

            builder
                .AddTransient<ISendGridClient>(provider =>
                {
                    var options = provider.GetRequiredService<IOptions<SendGridOptions>>();

                    return new SendGridClient(options.Value.ApiKey);
                })
                .AddTransient<IEmailSender, SendGridEmailSender>();

            return builder;
        }
    }
}
