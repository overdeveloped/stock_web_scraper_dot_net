using SeldonStockScannerAPI.Finviz_Company;

namespace SeldonStockScannerAPI.WatchList
{
    public class WatchListDTO
    {
        public int Id { get; set; }
        public string WatchListName { get; set; }
        public List<FinvizCompanyDTO> Companies { get; set; } = new();
    }
}
