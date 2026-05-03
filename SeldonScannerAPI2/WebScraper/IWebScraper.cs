
using SeldonStockScannerAPI.Finviz_Company;

namespace SeldonStockScannerAPI.WebScraper
{
    public interface IWebScraper
    {
        List<string> GetCompletePlus500();

        // Finviz website
        List<FinvizCompanyEntity> GetCustomWatchList(string url, string name);

        // Fidelity website
        Dictionary<string, string> getFTSE100();

        // Yahoo website
        double GetCurrentValue(string symbol);
    }
}
