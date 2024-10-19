using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BankAPI.Models
{
    public class Currency
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool Active { get; set; }


        public Currency (string name, string symbol, bool active)
        {
            Name = name;
            Symbol = symbol;
            Active = active;
        }
    }
}
