using BankAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace BankAPI.Services
{
    public class TransferService : ITransferService
    {
        private readonly IMongoCollection<Currency> _currencies;
        private readonly IMongoCollection<Wallet> _wallets;
        private readonly IMongoCollection<Transference> _transferenceHistory;
        private readonly ExternalCurrencyService _externalCurrencyService;
        private readonly IConfiguration _configuration;
        public TransferService(IOptions<MongoDBSettings> mongoDBSettings, IMongoClient mongoClient, ExternalCurrencyService externalCurrencyService, IConfiguration configuration)
        {
            var database = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _currencies = database.GetCollection<Currency>("Currency");
            _transferenceHistory = database.GetCollection<Transference>("TransferenceHistory");
            _wallets = database.GetCollection<Wallet>("Wallet");
            _externalCurrencyService = externalCurrencyService;
            _configuration = configuration;
        }

        public async Task<bool> PostTransference(TransferencePayload transference)
        {
            try
            {
                //var data = await _externalCurrencyService.GetCurrencyFromExternalApiAsync<dynamic>($"v1/latest?apikey={_configuration["ExtCurTk"]}&base_currency={transference.SourceWalletCurrency.Substring(0,3)}&currencies={transference.TargetWalletCurrency.Substring(0, 3)}");
                
                Wallet sourceWallet  = await _wallets.Find(_ => _._id == transference.SourceWalletId).FirstOrDefaultAsync();
                if (sourceWallet == null) {
                    throw new Exception("Carteira de saida não encontrada");
                }
                if ( Convert.ToDecimal(sourceWallet.Value) <=  Convert.ToDecimal(transference.ValuePlusFee)) {
                    throw new Exception("Saldo não suficiente");
                }

                Wallet targetWallet = await _wallets.Find(_ => _._id == transference.TargetWalletId).FirstOrDefaultAsync();
                if (sourceWallet == null)
                {
                    throw new Exception("Carteira de entrada não encontrada");
                }
                targetWallet.Value = (float.Parse(targetWallet.Value) + float.Parse(transference.Value)).ToString();
                sourceWallet.Value = (float.Parse(sourceWallet.Value) - float.Parse(transference.ValuePlusFee)).ToString();

                await _wallets.UpdateOneAsync(
                    w => w._id == transference.TargetWalletId,
                    Builders<Wallet>.Update.Set(w => w.Value, targetWallet.Value)
                );

                await _wallets.UpdateOneAsync(
                    w => w._id == transference.SourceWalletId,
                    Builders<Wallet>.Update.Set(w => w.Value, sourceWallet.Value)
                );

                Transference newTransference = new Transference
                (
                    transference.SourceWalletId,
                    transference.TargetWalletId,
                    transference.Value,
                    transference.ValuePlusFee,
                    transference.SourceWalletCurrency,
                    transference.TargetWalletCurrency,
                    transference.FeeCharged,
                    DateTime.Now
                );
                await _transferenceHistory.InsertOneAsync(newTransference);
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
        }

        public async Task<bool> PostDeposit(DepositPayload deposit)
        {
            try
            {
                Wallet sourceWallet = await _wallets.Find(_ => _._id == deposit.WalletId).FirstOrDefaultAsync();
                if (sourceWallet == null)
                {
                    throw new Exception("Carteira de entrada não encontrada");
                }
                sourceWallet.Value = (float.Parse(sourceWallet.Value) + float.Parse(deposit.Value)).ToString();

                await _wallets.UpdateOneAsync(
                    w => w._id == deposit.WalletId,
                    Builders<Wallet>.Update.Set(w => w.Value, sourceWallet.Value)
                );

                Transference newTransference = new Transference
                (
                    "Deposito",
                    deposit.WalletId,
                    deposit.Value,
                    deposit.ValuePlusFee,
                    "Deposito",
                    sourceWallet.CurrencyId,
                    deposit.FeeCharged,
                    DateTime.Now
                );
                await _transferenceHistory.InsertOneAsync(newTransference);
                return true;
            }
            catch (Exception ex) { 
                return false;
            }

        }
    }
}
