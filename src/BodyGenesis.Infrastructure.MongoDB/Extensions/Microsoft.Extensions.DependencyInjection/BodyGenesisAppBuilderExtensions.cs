using System;

using Microsoft.Extensions.Options;

using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

using BodyGenesis.Core.Entities;
using BodyGenesis.Infrastructure.MongoDB;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BodyGenesisAppBuilderExtensions
    {
        public static BodyGenesisAppBuilder UseMongoDB(this BodyGenesisAppBuilder builder, string connectionString, string databaseName)
        {
            builder
                .AddSingleton<IMongoClient>(provider =>
                {
                    var mongoDbOptions = provider.GetRequiredService<IOptions<MongoDBOptions>>();

                    return new MongoClient(connectionString);
                })
                .AddSingleton(provider =>
                {
                    var mongoDbOptions = provider.GetRequiredService<IOptions<MongoDBOptions>>();
                    var mongoClient = provider.GetRequiredService<IMongoClient>();

                    return mongoClient.GetDatabase(databaseName);
                })
                .AddTransient(typeof(IRepository<>), typeof(MongoDBRepository<>));

            BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));

            return builder;
        }
    }
}
