using System;

using Microsoft.Extensions.Options;
using MongoDB.Driver;

using BodyGenesis.Core.Entities;
using BodyGenesis.Infrastructure.MongoDB;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BodyGenesisAppBuilderExtensions
    {
        public static BodyGenesisAppBuilder UseMongoDB(this BodyGenesisAppBuilder builder, Action<MongoDBOptions> configure = null)
        {
            configure = configure ?? new Action<MongoDBOptions>(o => { });

            builder
                .AddOptions<MongoDBOptions>()
                .Bind(builder.Configuration.GetSection("BodyGenesis:Infrastructure:MongoDB"))
                .Configure(configure);

            builder
                .AddSingleton<IMongoClient>(provider =>
                {
                    var mongoDbOptions = provider.GetRequiredService<IOptions<MongoDBOptions>>();

                    return new MongoClient(mongoDbOptions.Value.ConnectionString);
                })
                .AddSingleton(provider =>
                {
                    var mongoDbOptions = provider.GetRequiredService<IOptions<MongoDBOptions>>();
                    var mongoClient = provider.GetRequiredService<IMongoClient>();

                    return mongoClient.GetDatabase(mongoDbOptions.Value.DatabaseName);
                })
                .AddTransient(typeof(IRepository<>), typeof(MongoDBRepository<>));

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));

            return builder;
        }
    }
}
