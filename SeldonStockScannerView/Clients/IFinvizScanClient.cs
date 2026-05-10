using SeldonStockScannerView.Models;

public interface IFinvizScanClient
{
    Task<List<FinvizCompanyModel>> GetAllAsync(string endpoint);
}
