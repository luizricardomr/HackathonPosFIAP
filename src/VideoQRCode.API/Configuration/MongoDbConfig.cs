using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace VideoQRCode.API.Configuration
{
    public static class MongoDbConfig
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(new DateTimeSerializer(DateTimeKind.Local));

            var mongoSection = configuration.GetSection("MongoDb");
            var connectionString = mongoSection["ConnectionString"];
            var databaseName = mongoSection["DatabaseName"];

            services.AddSingleton<IMongoClient>(sp =>
                new MongoClient(connectionString));

            services.AddSingleton<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(databaseName);
            });

            return services;
        }
    }
}
