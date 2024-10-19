using BankAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BankAPI.Services
{
    public class CurrencyService: ICurrencyService
    {
        private readonly IMongoCollection<Currency> _currencies;

        public CurrencyService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _currencies = database.GetCollection<Currency>("Currency");
        }

        public async Task<IEnumerable<Currency>> GetCurrencyList()
        {
            
            return await  _currencies.Find(_ => true).ToListAsync();
        }
    }
}
