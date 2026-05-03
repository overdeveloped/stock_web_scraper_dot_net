
namespace SeldonStockScannerAPI.Finviz_Company
{
    public interface IFinvizCompanyService
    {
        Task<IEnumerable<FinvizCompanyDTO>> GetAllAsync();
        Task<FinvizCompanyDTO?> GetByIdAsync(int id);
        Task<FinvizCompanyEntity> CreateAsync(FinvizCompanyEntity product);
        Task<FinvizCompanyEntity?> UpdateAsync(int id, FinvizCompanyEntity product);
        Task<bool> DeleteAsync(int id);
    }
}
