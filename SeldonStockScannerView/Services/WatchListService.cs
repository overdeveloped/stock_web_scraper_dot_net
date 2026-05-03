using SeldonStockScannerView.Models;
using static System.Net.WebRequestMethods;

namespace SeldonStockScannerView.Services
{
    public class WatchListService
    {
        private readonly HttpClient _httpClient;

        public WatchListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<WatchListModel>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<WatchListModel>>("WatchList");
        }

        public async Task<WatchListModel> Get(int id)
        {
            return await _httpClient.GetFromJsonAsync<WatchListModel>($"WatchList/{id}");
        }

        public async Task Create(WatchListModel watchList)
        {
            await _httpClient.PostAsJsonAsync("WatchList", watchList);
        }

        public async Task Update(WatchListModel watchList)
        {
            await _httpClient.PutAsJsonAsync($"WatchList/{watchList.WatchListId}", watchList);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"WatchList/{id}");
        }
    }
}
