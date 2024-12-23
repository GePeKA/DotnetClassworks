using Microsoft.Extensions.Caching.Distributed;
using MongoAPI.Data;
using MongoAPI.Domain;
using MongoDB.Driver;

namespace MongoAPI.Services
{
    public class EntityService(
        IDistributedCache redisCache,
        MongoDbContext dbContext) : IEntityService
    {
        public async Task<string> CreateOrReturnEntityIdAsync(Entity entity)
        {
            var idInRedis = await redisCache.GetStringAsync(entity.Key);
            if (idInRedis != null)
            {
                return idInRedis;
            }

            await dbContext.Entities.InsertOneAsync(entity);
            await redisCache.SetStringAsync(entity.Key, entity.Id);

            return entity.Id;
        }

        public async Task<string?> DeleteEntityByKeyAsync(string key)
        {
            var idInRedis = await redisCache.GetStringAsync(key);
            if (idInRedis == null)
            {
                return null;
            }

            await dbContext.Entities.DeleteOneAsync(e => e.Key == key);
            await redisCache.RemoveAsync(key);

            return idInRedis;
        }

        public async Task<string?> GetEntityIdByKeyAsync(string key)
        {
            return await redisCache.GetStringAsync(key);
        }
    }
}
