using MongoAPI.Domain;

namespace MongoAPI.Services
{
    public interface IEntityService
    {
        Task<string?> GetEntityIdByKeyAsync(string key);
        Task<string> CreateOrReturnEntityIdAsync(Entity entity);
        Task<string?> DeleteEntityByKeyAsync(string key); 
    }
}
