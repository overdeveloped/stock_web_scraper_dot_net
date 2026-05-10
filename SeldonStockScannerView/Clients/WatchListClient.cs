using SeldonStockScannerView.Models;
using static System.Net.WebRequestMethods;

namespace SeldonStockScannerView.Services
{
    public class WatchListClient : IWatchListClient
    {
        private readonly HttpClient _httpClient;

        public WatchListClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WatchListModel>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<WatchListModel>>("FinvizWatchList")
                ?? new List<WatchListModel>();
        }

        public async Task<WatchListModel?> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<WatchListModel>($"FinvizWatchList/{id}");
        }

        public async Task<WatchListModel> CreateAsync(WatchListModel entity)
        {
            var response = await _httpClient.PostAsJsonAsync("FinvizWatchList", entity);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WatchListModel>();
        }

        public async Task<WatchListModel> CreateWithCompaniesAsync(WatchListModel dto)
        {
            var response = await _httpClient.PostAsJsonAsync("FinvizWatchList/create-with-companies", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WatchListModel>();
        }

        public async Task<WatchListModel?> UpdateAsync(int id, WatchListModel entity)
        {
            var response = await _httpClient.PutAsJsonAsync($"FinvizWatchList/{id}", entity);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<WatchListModel>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"FinvizWatchList/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
