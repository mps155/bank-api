using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankAPI.Models
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Wallets { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
    }

    public class RegisterOwnerRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
    }
}
