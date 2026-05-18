using SeldonStockScannerView.Models;

public interface IFinvizCompanyClient   
{
    Task<FinvizCompanyModel?> CheckOrAdd(int ticker);
}
