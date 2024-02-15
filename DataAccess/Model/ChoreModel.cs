using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Model
{
    public class ChoreModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        public string ChoreText { get; set; }
        public int FreqencyOfChores { get; set; }
        public UserModel? AssignedTo { get; set; }
        public DateTime? LastCompleted { get; set; }
    }
}
