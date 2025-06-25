using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CryptoPulse.Application.Features.Rates.Interfaces;

namespace CryptoPulse.Application.Features.Rates.Services
{
    public class RatesService : IRatesService
    {
        private readonly HttpClient _httpClient;

        public RatesService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetCoinRatesAsync()
        {
            string url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum&vs_currencies=usd";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}