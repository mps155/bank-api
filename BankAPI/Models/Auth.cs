using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BankAPI.Models
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsRevoked { get; set; }
    }

    public class JwtConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiresInMinutes { get; set; }
    }

}
