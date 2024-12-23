using MongoAPI.Domain;
using MongoDB.Driver;

namespace MongoAPI.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Entity> Entities => _database.GetCollection<Entity>("entities");
}
