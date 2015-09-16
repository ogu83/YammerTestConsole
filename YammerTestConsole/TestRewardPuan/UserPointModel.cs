
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
namespace TestRewardPuan
{
    public class UserPointModel
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        public string userName { get; set; }
        public int userId { get; set; }
        public double point { get; set; }
        public string desc { get; set; }
    }
}