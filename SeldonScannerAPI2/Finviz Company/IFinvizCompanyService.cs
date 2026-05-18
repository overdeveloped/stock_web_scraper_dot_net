
namespace SeldonStockScannerAPI.Finviz_Company
{
    public interface IFinvizCompanyService
    {
        Task<IEnumerable<FinvizCompanyDTO>> GetAllAsync();
        Task<FinvizCompanyDTO?> GetByIdAsync(int id);
        Task<FinvizCompanyDTO?> GetByTickerAsync(string ticker);
        Task<FinvizCompanyEntity> CreateAsync(FinvizCompanyEntity company);
        Task<FinvizCompanyEntity?> UpdateAsync(string id, FinvizCompanyEntity company);
        Task<bool> DeleteAsync(int id);
    }
}
