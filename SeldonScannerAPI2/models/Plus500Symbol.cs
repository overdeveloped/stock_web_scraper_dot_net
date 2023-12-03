//using Newtonsoft.Json;

namespace SeldonStockScannerAPI.models
{
    // The property names reflect the column titles on the Finviz results page
    public class Plus500Symbol
    {
        public int Id { get; set; }
        public string Ticker { get; set; }

        public Plus500Symbol()
        {
        }
    }
}
