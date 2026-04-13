using SeldonStockScannerAPI.Models;

namespace SeldonStockScannerAPI.WatchList
{
    public interface IWatchListService
    {
        List<WatchListEntity> GetWatchList() { return new List<WatchListEntity>(); }
        void AddWatchItem(WatchListEntity watchItem);

    }
}
