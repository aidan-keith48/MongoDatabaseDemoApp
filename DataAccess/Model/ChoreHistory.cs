using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DataAccess.Model
{
    public class ChoreHistory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string choreID { get; set; }
        public string ChoreText { get; set; }

        public DateTime? DateCompleted { get; set; }
        public UserModel? AssignedTo { get; set; }

        public ChoreHistory()
        {

        }

        public ChoreHistory(ChoreModel chores)
        {
            choreID = chores.Id;
            DateCompleted = chores.LastCompleted ?? DateTime.Now;
            AssignedTo = chores.AssignedTo;
            ChoreText = chores.ChoreText;
        }
    }
}
