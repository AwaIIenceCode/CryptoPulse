using System.Threading.Tasks;

namespace CryptoPulse.Application.Features.Rates.Interfaces
{
    public interface IRatesService
    {
        Task<string> GetCoinRatesAsync();
    }
}