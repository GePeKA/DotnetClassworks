namespace MongoAPI.Extensions.Redis;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["CacheSettings:RedisConfiguration"];
        });

        return services;
    }
}
