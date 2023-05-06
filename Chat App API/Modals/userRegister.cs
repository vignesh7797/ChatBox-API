using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Chat_App_API.Modals
{
    public class userRegister
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public long Phone { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Story { get; set; } = String.Empty;
        public string? Img { get; set; }
        public Boolean IsOnline { get; set; }
        public string Password { get; set; } = String.Empty;

        public List<string> Friends { get; set; } = new List<string>();
    }
}
