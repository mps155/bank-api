using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankAPI.Models
{
    public class Wallet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string OwnerId { get; set; }
        public string CurrencyId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }


        public Wallet(string ownerId, string currencyId, string name, string value, bool active)
        {
            OwnerId = ownerId;
            CurrencyId = currencyId;
            Name = name;
            Value = value;
            Active = active;
        }


    }
}
