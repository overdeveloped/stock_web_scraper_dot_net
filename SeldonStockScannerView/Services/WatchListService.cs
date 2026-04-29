using SeldonStockScannerView.Models;

namespace SeldonStockScannerView.Services
{
    public class WatchListService
    {
        private readonly HttpClient _httpClient;

        public WatchListService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7059");
        }

        public async Task<List<WatchList>> GetAll()
        {
            return await _httpClient.GetFromJsonAsync<List<WatchList>>("api/WatchList");
        }

        public async Task<WatchList> Get(int id)
        {
            return await _httpClient.GetFromJsonAsync<WatchList>($"api/WatchList/{id}");
        }

        public async Task Create(WatchList watchList)
        {
            await _httpClient.PostAsJsonAsync("api/WatchList", watchList);
        }

        public async Task Update(WatchList watchList)
        {
            await _httpClient.PutAsJsonAsync($"api/WatchList/{watchList.WatchListId}", watchList);
        }

        public async Task Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/WatchList/{id}");
        }
    }
}
