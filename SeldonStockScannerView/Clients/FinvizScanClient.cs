using SeldonStockScannerView.Models;

namespace SeldonStockScannerView.Clients
{
    public class FinvizScanClient : IFinvizScanClient
    {
        private readonly HttpClient _httpClient;

        public FinvizScanClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<FinvizCompanyModel>> GetAllAsync(string endpoint)
        {
            // URL encode the string to avoid issues with spaces, symbols, etc.
            var encoded = Uri.EscapeDataString(endpoint);

            return await _httpClient.GetFromJsonAsync<List<FinvizCompanyModel>>(endpoint)
                ?? new List<FinvizCompanyModel>();
        }
    }
}
