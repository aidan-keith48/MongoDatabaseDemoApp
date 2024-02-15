using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;



namespace MongoDatabaseDemoApp.Model
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string jobDescription { get; set; }
    }
}
