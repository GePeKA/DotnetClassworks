using MongoAPI.Data;

namespace MongoAPI.Extensions.MongoDB
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbContext = new MongoDbContext(
                configuration.GetConnectionString("MongoDbConnection")!,
                configuration["MongoSettings:MongoDbName"]!
            );

            return services
                .AddSingleton(mongoDbContext);
        }
    }
}
