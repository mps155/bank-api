using BankAPI.Models;

namespace BankAPI.Services
{
    public interface ITransferService
    {
        Task<bool> PostTransference(TransferencePayload transference);
        Task<bool> PostDeposit(DepositPayload deposit);
    }
}
