//using Newtonsoft.Json;

namespace SeldonStockScannerAPI.models
{
    // The property names reflect the column titles on the Finviz results page
    public class FinvizCompanyEntity
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public string Company { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public string Country { get; set; }
        public string MarketCap { get; set; }
        public string PE { get; set; }
        public string Price { get; set; }
        public string Change { get; set; }
        public string Volume { get; set; }

        public FinvizCompanyEntity()
        {

        }
    }
}
