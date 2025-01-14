using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class SeniorFem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name {get; set;}

        [BsonElement("mote")]
        public string Mote {get; set;}

        [BsonElement("image")]
        public string Image {get; set;}
    }
}