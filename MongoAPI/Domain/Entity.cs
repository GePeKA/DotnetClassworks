using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoAPI.Domain
{
    public class Entity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Key { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = null!;
    }
}
