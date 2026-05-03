using SeldonStockScannerAPI.Models;
using SeldonStockScannerAPI.WatchList;

namespace SeldonStockScannerAPI.Finviz_Company
{
    public interface IFinvizCompanyService
    {
        Task<IEnumerable<FinvizCompanyEntity>> GetAllAsync();
        Task<FinvizCompanyEntity?> GetByIdAsync(int id);
        Task<FinvizCompanyEntity> CreateAsync(FinvizCompanyEntity product);
        Task<FinvizCompanyEntity?> UpdateAsync(int id, FinvizCompanyEntity product);
        Task<bool> DeleteAsync(int id);
    }
}
