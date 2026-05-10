
namespace SeldonStockScannerAPI.Finviz_Company
{
    public interface IFinvizCompanyService
    {
        Task<IEnumerable<FinvizCompanyDTO>> GetAllAsync();
        Task<FinvizCompanyDTO?> GetByIdAsync(int id);
        Task<FinvizCompanyEntity> CreateAsync(FinvizCompanyEntity company);
        Task<FinvizCompanyEntity?> UpdateAsync(int id, FinvizCompanyEntity company);
        Task<bool> DeleteAsync(int id);
    }
}
