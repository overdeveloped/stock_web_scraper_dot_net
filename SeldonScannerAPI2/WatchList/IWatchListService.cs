namespace SeldonStockScannerAPI.WatchList
{
    public interface IWatchListService
    {
        Task<IEnumerable<WatchListDTO>> GetAllAsync();
        Task<WatchListDTO?> GetByIdAsync(int id);
        Task<WatchListEntity> CreateAsync(WatchListEntity product);
        Task<WatchListEntity> CreateWithCompaniesAsync(AttachCompaniesDTO request);
        Task<WatchListEntity> AddCompanyToWatchListAsync(AttachCompaniesDTO request);
        Task<WatchListEntity?> UpdateAsync(int id, WatchListEntity product);
        Task<bool> DeleteAsync(int id);
    }
}
