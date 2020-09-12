using System;

using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBodyGenesis(this IServiceCollection services, IConfiguration configuration, Action<BodyGenesisAppBuilder> buildApp)
        {
            var builder = new BodyGenesisAppBuilder(services, configuration);

            buildApp(builder);

            return services;
        }
    }
}
