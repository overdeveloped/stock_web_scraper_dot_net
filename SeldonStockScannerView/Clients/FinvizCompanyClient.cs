using SeldonStockScannerView.Models;
using System.Net.Http;

namespace SeldonStockScannerView.Clients
{
    public class FinvizCompanyClient : IFinvizCompanyClient
    {
        private readonly HttpClient _httpClient;

        public FinvizCompanyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<FinvizCompanyModel?> CheckOrAdd(int ticker)
        {
            var response = await _httpClient.PostAsJsonAsync("/check-or-add", ticker);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FinvizCompanyModel>();
        }
    }
}
