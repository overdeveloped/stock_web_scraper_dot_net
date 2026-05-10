using SeldonStockScannerView.Models;

public interface IWatchListClient
{
    Task<List<WatchListModel>> GetAllAsync();
    Task<WatchListModel?> GetByIdAsync(int id);
    Task<WatchListModel> CreateAsync(WatchListModel entity);
    Task<WatchListModel> CreateWithCompaniesAsync(WatchListModel dto);
    Task<WatchListModel?> UpdateAsync(int id, WatchListModel entity);
    Task<bool> DeleteAsync(int id);
}
