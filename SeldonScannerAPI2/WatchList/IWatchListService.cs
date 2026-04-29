namespace SeldonStockScannerAPI.WatchList
{
    public interface IWatchListService
    {
        IEnumerable<WatchListEntity> GetAll();
        Task<WatchListEntity?> GetByIdAsync(int id);
        Task<WatchListEntity> CreateAsync(WatchListEntity product);
        Task<WatchListEntity?> UpdateAsync(int id, WatchListEntity product);
        Task<bool> DeleteAsync(int id);




        //List<WatchListEntity> GetWatchList() { return new List<WatchListEntity>(); }
        //void AddWatchItem(WatchListEntity watchItem);

    }
}
