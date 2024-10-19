using BankAPI.Models;

namespace BankAPI.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> GetCurrencyList();
    }
}
