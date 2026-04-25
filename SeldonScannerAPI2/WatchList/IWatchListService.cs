using SeldonStockScannerAPI.Models;

namespace SeldonStockScannerAPI.WatchList
{
    public interface IWatchListService
    {
        Task<IEnumerable<WatchListEntity>> GetAllAsync();
        Task<WatchListEntity?> GetByIdAsync(int id);
        Task<WatchListEntity> CreateAsync(WatchListEntity product);
        Task<WatchListEntity?> UpdateAsync(int id, WatchListEntity product);
        Task<bool> DeleteAsync(int id);




        //List<WatchListEntity> GetWatchList() { return new List<WatchListEntity>(); }
        //void AddWatchItem(WatchListEntity watchItem);

    }
}
