using BankAPI.Models;

namespace BankAPI.Services
{
    public interface IWalletService
    {
        Task<IEnumerable<Wallet>> GetWalletList(string walletOwner);
        Task<Wallet> GetWallet(string walletId);
    }
}
