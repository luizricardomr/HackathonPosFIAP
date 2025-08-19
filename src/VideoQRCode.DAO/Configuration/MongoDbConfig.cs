using MongoDB.Driver;

namespace VideoQRCode.DAO.Configuration
{
    public static class MongoDbConfig
    {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
        {
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
