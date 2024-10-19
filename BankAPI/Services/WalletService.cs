using BankAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BankAPI.Services
{
    public class WalletService: IWalletService
    {
        private readonly IMongoCollection<Wallet> _wallets;

        public WalletService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _wallets = database.GetCollection<Wallet>("Wallet");
        }

        public async Task<IEnumerable<Wallet>> GetWalletList(string walletOwner)
        {
            return await _wallets.Find(_ => _.OwnerId ==  walletOwner).ToListAsync();
        }
        public async Task<Wallet> GetWallet(string walletId)
        {
            return await _wallets.Find(_ => walletId.Equals(_._id)).FirstOrDefaultAsync();
        }
    }
}
