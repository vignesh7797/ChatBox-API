using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chat_App_API.Modals
{
    public class chatBox
    {
        [BsonId]
        public ObjectId? Id { get; set; }
        public int UserId{ get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; }
        public string? Text { get; set; }
    }
}
